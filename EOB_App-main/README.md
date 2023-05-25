# EOB_App
This repository contains **EOB Engineers App** and **EOB Website**

## Building EOB Engineers App
1. Install WiX Toolset: <https://wixtoolset.org/releases/>.
1. In Visual Studio, select Release configuration.
2. x64 is preferred but you can also select x86.
3. Right click **Engineers_App.Setup** and click on Build
4. Once building is complete, right click on **Engineers_App.Setup_Bundle** and click on Build. This will combine the ZeroTier installer with our own.
5. The installer should be located in **Engineers_App.Setup_Bundle**/bin/Release/

## Publishing the web app

1. Go to <https://control.mijnhostingpartner.nl>.
2. On the navigation tree, go to Web -> Web Sites.
3. You will have to stop either app.engineeroutofthebox.com or staging.engineeroutofthebox.com depending on your needs.
4. Locate the Powershell script in EOB_App\EOB_Web_App\publish.ps1
- For the production (master) site, run: `./publish.ps1 production`
- For the staging (development) site, run: `./publish.ps1 staging`
- To only update the downloads, add `-downloadsOnly` at the end: `./publish.ps1 production -downloadsOnly`