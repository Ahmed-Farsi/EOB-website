<#
  .SYNOPSIS
    Engineeroutofthebox.com publish script
  .DESCRIPTION
    The built in Visual Studio publishing method absolutley sucks, so I made this script with the use of WinSCP to synchronize the files which is also MUCH faster.
    The only thing you need is to install WinSCP and then run this script through powershell. 
    Use -env "staging" for the development site or -env "production" for the main site.
    You can also use -downloadsOnly to skip synchronizing the website and obly synchronize the downloads

    BTW, I lost all my sanity making this script. This truly is the worst syntax ever.
  .EXAMPLE
    ./publish.ps1 -env [staging|production]
    ./publish.ps1 -env [staging|production] -downloadsOnly
#>

param 
(
    [Parameter(Mandatory = $true)]
    $env,
    [switch]$downloadsOnly
)

#---------------------------------------------------------------------------------------

#paths
$WINSCP_PATH = "${Env:ProgramFiles(x86)}\WinSCP\WinSCPnet.dll"
$PROJECT_PATH = "$(Get-Location)\Eob_Web.Frontend\Eob_Web.Frontend.csproj"
$BUILD_PATH = "$(Get-Location)\Eob_Web.Frontend\bin\Release\net6.0\publish"
$DOWNLOADS_PATH = "Z:\01 Operations\01 projecten\78000 intern\P78048 EOB\777 SoftwareFiles"

# ftp
$REMOTE = "ftp.engineeroutofthebox.com"
$DOMAIN = (&{if($env -Contains "production") {"app"} else {"staging"}}) + ".engineeroutofthebox.com"
$USERNAME = "ftppro-control-admin"
$PAGETITLE = "Engineer Out of the Box"

#---------------------------------------------------------------------------------------

function IncludeWinScp()
{
    if(!(Test-path $WINSCP_PATH -PathType leaf))
    {
        return $false
    }

    Add-Type -Path $WINSCP_PATH
    return $true
}

Function WebsiteOnline()
{
    try
    {
        $request = Invoke-WebRequest -Uri $DOMAIN
        if ($request.StatusCode -ne 200)
        {
            return $false
        }

        # Weird check but needed because sometimes I still get 200 when I have clearly turned off the website.
        if ($request.ParsedHtml.Title -ne $PAGETITLE)
        {
            return $false
        }
    }
    catch
    {
        return $false
    }

    return $true
}

function Build()
{
    dotnet publish $PROJECT_PATH -c Release /p:EnvironmentName=$env | Write-Host
    if ($LASTEXITCODE -ne 0)
    {
        return $false
    }

    return $true
}

Function CompareFiles([WinSCP.Session]$session, [string]$localPath, [string]$REMOTEPath, [string]$fileMask = $null)
{
    $transferOptions = New-Object WinSCP.TransferOptions
    $transferOptions.FileMask = $fileMask

    $res = $session.CompareDirectories(
        [WinSCP.SynchronizationMode]::Remote, 
        $localPath,
        $REMOTEPath,
        $true,
        $false,
        [WinSCP.SynchronizationCriteria]::Either,
        $transferOptions)

	return $res
}

Function SynchronizeFiles([WinSCP.Session]$session, [string]$localPath, [string]$REMOTEPath, [string]$fileMask = $null)
{
    $transferOptions = New-Object WinSCP.TransferOptions
    $transferOptions.FileMask = $fileMask

    $res = $session.SynchronizeDirectories(
        [WinSCP.SynchronizationMode]::Remote, 
        $localPath,
        $REMOTEPath,
        $true,
        $false,
        [WinSCP.SynchronizationCriteria]::Either,
        $transferOptions)

    if (!($res.IsSuccess))
    {
        return $false
    }

    return $true
}

$script:transfer = $Null
function OnFileTransfer($e)
{
    # New line for every new file
    if (($script:transfer -ne $Null) -and
        ($script:transfer -ne $e.FileName))
    {
        Write-Host
    }
 
    # Print transfer progress
    Write-Host -NoNewline ("`r$($e.FileName.Replace("$(Get-Location)\", ''))")
 
    # Remember a name of the last file reported
    $script:transfer = $e.FileName
}

#---------------------------------------------------------------------------------------

function SyncWebsite([WinSCP.Session]$session)
{
	Write-Host -ForegroundColor Yellow "Building project..."
	if (!(Build))
	{
		$session.Dispose()
		exit
	}

    #compare
	Write-Host -ForegroundColor Yellow "`r`nComparing files..."
    $files = CompareFiles $session $BUILD_PATH $DOMAIN "*; */ | data/; logs/; SoftwareFiles/"
	if ($files.Count -ne 0)
    {
		Write-Host -ForegroundColor Yellow "Here is a list of items that will be modified/added/removed:`r`n"
		foreach ($file in $files)
		{
			Write-Host " - $($file.ToString().Replace("$(Get-Location)\", ''))"
		}
    }
    else
    {
		Write-Host -ForegroundColor Yellow "`r`nThere are no new changes."
        return
    }

	#sync
	Write-Host -ForegroundColor Yellow "`r`nSynchronizing files..."
	if (!(SynchronizeFiles $session $BUILD_PATH $DOMAIN "*; */ | data/; logs/; SoftwareFiles/"))
	{
		Write-Host -ForegroundColor Red "`r`nFailed synchronizing files! Make sure the website is off."
		$session.Dispose()
		exit
	}

	Write-Host -ForegroundColor Green "`r`nSuccessfully synchronized files!"
}

function SyncDownloads([WinSCP.Session]$session)
{
    #compare
    Write-Host -ForegroundColor Yellow "`r`nComparing downloads..."
    
    $files = CompareFiles $session $DOWNLOADS_PATH "$DOMAIN/wwwroot/SoftwareFiles"
	if ($files.Count -ne 0)
    {
		Write-Host -ForegroundColor Yellow "Here is a list of items that will be modified/added/removed:`r`n"
		foreach ($file in $files)
		{
			Write-Host " - $($file.ToString().Replace("$(Get-Location)\", ''))"
		}
    }
    else
    {
		Write-Host -ForegroundColor Yellow "There are no new changes."
        return
    }

    #sync downloads
    Write-Host -ForegroundColor Yellow "`r`nSynchronizing downloads..."
    if (!(SynchronizeFiles $session $DOWNLOADS_PATH "$DOMAIN/wwwroot/SoftwareFiles"))
    {
        Write-Host  -ForegroundColor Red "`r`nFailed synchronizing files! Make sure the website is off."
        $session.Dispose()
        exit
    }

    Write-Host -ForegroundColor Green "`r`nSuccessfully synchronized downloads!"
}

#---------------------------------------------------------------------------------------

function Main()
{
    if ("production","staging" -NotContains $env)
    {
        Write-Host "Incorrect parameters! make sure to read description."
        Get-Help $MyInvocation.MyCommand.Definition
        exit
    }

    if (!(IncludeWinScp))
    {
        Write-Host "WinSCP is not installed. Please install it."
        exit
    }

    if (WebsiteOnline)
    {
        Write-Host "The website is still online! Please turn the website off."
        exit
    }

    $password = Read-Host "Enter password for $USERNAME (right-click to paste)" -AsSecureString
    $password = [Runtime.InteropServices.Marshal]::PtrToStringAuto([Runtime.InteropServices.Marshal]::SecureStringToBSTR($password))

    try
    {
        $sessionOptions = New-Object WinSCP.SessionOptions
        $sessionOptions.Protocol = [WinSCP.Protocol]::Ftp
        $sessionOptions.HostName = $DOMAIN
        $sessionOptions.UserName = $USERNAME
        $sessionOptions.Password = $password
        $sessionOptions.FtpSecure = [WinSCP.FtpSecure]::Explicit
        $sessionOptions.GiveUpSecurityAndAcceptAnyTlsHostCertificate = $true

        $session = New-Object WinSCP.Session
        $session.add_FileTransferProgress( { OnFileTransfer($_) } )
        $session.Open($sessionOptions)
    }
    catch
    {
        Write-Host "Error: $($_.Exception.Message)"
        $session.Dispose()
        exit
    }
    
    if (!($downloadsOnly))
    {
		#$session is automatically passed as reference
        SyncWebsite($session)
    }

    SyncDownloads($session)

    $session.Dispose()
}

Main
