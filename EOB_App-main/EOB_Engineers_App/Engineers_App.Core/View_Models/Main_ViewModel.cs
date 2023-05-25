using Engineers_App.Core.Services;
using Engineers_App.Core.Models;
using Engineers_App.Core.Models.ZeroTier;
using Engineers_App.Core.View_Properties;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Engineers_App.Core.View_Models
{
    public class Main_ViewModel : MvxNavigationViewModel
    {
        private string _token_Path = 
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + $"\\EOB\\token.cfg";

        private readonly IZeroTier_Service _zeroTier_Service;
        private readonly IEob_Service _eob_Service;
        private readonly ILogger<Main_ViewModel> _logger;
        private readonly IMvxNavigationService _navigationService;
        private readonly IConfiguration _config;

        public Menu Menu { get; set; }

        public Main_ViewModel(
            ILoggerFactory loggerFactory,
            IMvxNavigationService navigationService,
            IZeroTier_Service zerotier_Api,
            IEob_Service eob_Api,
            ILogger<Main_ViewModel> logger,
            IConfiguration config,
            Menu menu)
            : base(loggerFactory, navigationService)
        {
            Close_Command = new MvxAsyncCommand(() => navigationService.Close(this));
            Show_Networks_Command = new MvxAsyncCommand(() => navigationService.Navigate<Networks_ViewModel>());
            Show_Personal_Command = new MvxAsyncCommand(() => navigationService.Navigate<Personal_ViewModel>());
            Show_Eobs_Command = new MvxAsyncCommand(() => navigationService.Navigate<Eobs_ViewModel>());
            Show_Settings_Command = new MvxAsyncCommand(() => navigationService.Navigate<Settings_ViewModel>());

            _zeroTier_Service = zerotier_Api;
            _eob_Service = eob_Api;
            _logger = logger;
            _navigationService = navigationService;
            _config = config;
            Menu = menu;

            Menu.Menu_Open = true;
            Menu.Menu_Enabled = true;
            App_Version = config["App_Version"];

            // TODO: UI crashes when calling Get_Status() from Initialize()
            Task.Run(async () =>
            {
                Status = await _zeroTier_Service.Get_Status();
                await _navigationService.Navigate<Home_ViewModel>();
                await AutoLogin();
            });
        }

        public override async Task Initialize()
        {
        }

        #region View properties

        public IMvxAsyncCommand Close_Command { get; }
        public IMvxAsyncCommand Show_Networks_Command { get; }
        public IMvxAsyncCommand Show_Eobs_Command { get; }
        public IMvxAsyncCommand Show_Personal_Command { get; }
        public IMvxAsyncCommand Show_Settings_Command { get; }
        public IMvxAsyncCommand Login_Command => new MvxAsyncCommand(Login, Login_Allowed);
        public IMvxAsyncCommand Logout_Command => new MvxAsyncCommand(Logout);

        private ZT_Status _status;
        public ZT_Status Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        private bool _is_Logged_In;
        public bool Is_Logged_In
        {
            get => _is_Logged_In;
            set => SetProperty(ref _is_Logged_In, value);
        }

        private string _email_Address;
        public string Email_Address
        {
            get => _email_Address;
            set => SetProperty(ref _email_Address, value); 
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private string _app_Version;
        public string App_Version
        {
            get => _app_Version;
            set => SetProperty(ref _app_Version, value); 
        }

        private string _welcome_Text;
        public string Welcome_Text
        {
            get => _welcome_Text;
            set => SetProperty(ref _welcome_Text, value); 
        }

        private string _login_Status;
        public string Login_Status
        {
            get => _login_Status;
            set => SetProperty(ref _login_Status, value); 
        }

        private bool _remember_Login;
        public bool Remember_Login
        {
            get => _remember_Login;
            set => SetProperty(ref _remember_Login, value); 
        }

        #endregion

        private async Task Logout()
        {
            File.WriteAllText(_token_Path, string.Empty);
            Welcome_Text = string.Empty;
            Is_Logged_In = false;
            await _navigationService.Navigate<Home_ViewModel>();
        }

        private async Task Login()
        {
            var result = await _eob_Service.Login(Email_Address, Password);
            if (result == null)
            {
                switch (_eob_Service.Status_Code)
                {
                    case HttpStatusCode.Unauthorized:
                        Login_Status = "Invalid credentials";
                        break;

                    case HttpStatusCode.Forbidden:
                        Login_Status = "You don't have the right privileges to login here";
                        break;

                    case HttpStatusCode.BadRequest:
                        Login_Status = "There is no company attached to your account";
                        break;

                    default:
                        Login_Status = "Server-side error occurred";
                        break;
                }

                return;
            }

            if (_remember_Login)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_token_Path));
                File.WriteAllText(_token_Path, result.Token);
            }

            Welcome_Text = $"Welcome, {result.User.Email_Address}";
            Is_Logged_In = true;
            Login_Status = string.Empty;
            await Leave_All_Networks();
        }

        private bool Login_Allowed()
        {
            return true;
        }

        private async Task Leave_All_Networks()
        {
            string zeroTierPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                    "ZeroTier",
                    "One",
                    "networks.d");

            var files = Directory.GetFiles(zeroTierPath).ToList();
            files.RemoveAll(x => x.Contains(".local.conf"));

            foreach (string file in files)
            { 
                await _eob_Service.ZT_Node_Reset(Path.GetFileNameWithoutExtension(file), Status.address);
                await _zeroTier_Service.Leave_Network(Path.GetFileNameWithoutExtension(file));
            }
        }

        private async Task AutoLogin()
        {
            if (!File.Exists(_token_Path))
                return;

            var info = new FileInfo(_token_Path);
            if (info.Length == 0)
                return;

            string token = File.ReadAllText(_token_Path);
            var result = await _eob_Service.Get_User(token);

            if (result == null)
            {
                File.WriteAllText(_token_Path, string.Empty);
                return;
            }

            Welcome_Text = $"Welcome, {result.Email_Address}";
            Is_Logged_In = true;
            Login_Status = string.Empty;
            await Leave_All_Networks();
        }
    }
}

