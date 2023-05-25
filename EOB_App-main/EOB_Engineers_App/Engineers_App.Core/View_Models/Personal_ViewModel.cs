using Engineers_App.Core.Services;
using Engineers_App.Core.Models;
using Engineers_App.Core.Models.ZeroTier;
using Engineers_App.Core.View_Properties;
using Engineers_App.Core.Webcam;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using OpenCvSharp;
using Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Engineers_App.Core.View_Models
{
    public class Personal_ViewModel_Parameters
    {
        public Eob Eob { get; set; }
        public Route[] Routes { get; set; }
    }

    public class Personal_ViewModel : MvxViewModel<Personal_ViewModel_Parameters>, IDisposable
    {
        private Webcam_Handler _webcam_Handler;
        private Network_Socket _network_Socket;
        private Timer _timer;
        private IZeroTier_Service _zeroTier_Service;
        private IEob_Service _eob_Service;
        private IMvxNavigationService _navigation_Service;
        private IConfiguration _config;
        private ILogger<Personal_ViewModel> _logger;

        public Menu Menu { get; set; }

        public Personal_ViewModel(
            IZeroTier_Service zeroTier_Api,
            IEob_Service eob_Api,
            IMvxNavigationService navigation_Service,
            IConfiguration config,
            ILogger<Personal_ViewModel> logger,
            Menu menu)
        {
            _zeroTier_Service = zeroTier_Api;
            _eob_Service = eob_Api;
            _navigation_Service = navigation_Service;
            _config = config;
            _logger = logger;
            Menu = menu;

            Menu.Menu_Enabled = false;
            Menu.Menu_Open = false;

            Buttons_Enabled = true;
            Ip_List = new MvxObservableCollection<string>();
            Routes = new MvxObservableCollection<string>();
        }

        public override async void Prepare(Personal_ViewModel_Parameters parameters)
        {
            Eob = parameters.Eob;

            foreach (var route in parameters.Routes)
            { 
                Routes.Add(route.target);
            }

            Eob_Ip_Address = _config.GetValue<string>("EOB_Ip");
            await Scan_Ip_Addresses();
        }

        #region View properties

        public IMvxAsyncCommand Start_Camera_Command => new MvxAsyncCommand(Start_Camera);
        public IMvxCommand Close_Camera_Command => new MvxCommand(Close_Camera);
        public IMvxAsyncCommand Ip_Scan_Command => new MvxAsyncCommand(Scan_Ip_Addresses);
        public IMvxAsyncCommand Close_Connection_Command => new MvxAsyncCommand(Close_Connection);
        public IMvxAsyncCommand Add_Subnet_Command => new MvxAsyncCommand(Add_Subnet);
        public IMvxAsyncCommand<string> Delete_Subnet_Command => new MvxAsyncCommand<string>(x => Delete_Subnet(x));

        private byte[] _local_Cam;
        public byte[] Local_Cam
        {
            get => _local_Cam;
            set => SetProperty(ref _local_Cam, value);
        }
         
        private byte[] _remote_Cam;
        public byte[] Remote_Cam
        {
            get => _remote_Cam;
            set => SetProperty(ref _remote_Cam, value);
        }

        private string _status_Message;
        public string Status_Message
        {
            get => _status_Message;
            set => SetProperty(ref _status_Message, value);
        }

        private bool _buttons_Enabled;
        public bool Buttons_Enabled
        {
            get => _buttons_Enabled;
            set => SetProperty(ref _buttons_Enabled, value);
        }

        private MvxObservableCollection<string> _ip_List;
        public MvxObservableCollection<string> Ip_List 
        {
            get => _ip_List;
            set => SetProperty(ref _ip_List, value);
        }

        private string _eob_Ip_Address;
        public string Eob_Ip_Address
        {
            get =>  _eob_Ip_Address;
            set => SetProperty(ref _eob_Ip_Address, value);
        }

        private MvxObservableCollection<string> _routes;
        public MvxObservableCollection<string> Routes
        {
            get =>  _routes;
            set => SetProperty(ref _routes, value);
        }

        private Eob _eob;
        public Eob Eob
        {
            get =>  _eob;
            set => SetProperty(ref _eob, value);
        }

        private string _subnet;
        public string Subnet
        {
            get => _subnet;
            set => SetProperty(ref _subnet, value);
        }

        private string _mask;
        public string Mask 
        {
            get => _mask;
            set => SetProperty(ref _mask, value);
        }

        #endregion

        private async Task Start_Camera()
        {
            Buttons_Enabled = false;
            _webcam_Handler = new Webcam_Handler();

            if (!_webcam_Handler.Start())
            { 
                Status_Message = "There is no webcam plugged in.";
                Buttons_Enabled = true;
                Dispose();
                return;
            }

            _timer = new Timer(
                callback: Read_Local_Cam,
                state: null,
                dueTime: 1000,
                period: 33);

            Status_Message = "Connecting...";
            _network_Socket = new Network_Socket(Eob_Ip_Address, 16896);
            _network_Socket.Data_Received += On_Data_Recieved;
            _network_Socket.Connected += On_Connected;
            _network_Socket.Disconnected += On_Disconnected;
            _network_Socket.Server_Accepted += On_Server_Accepted;

            if (!await _network_Socket.Start())
            { 
                Status_Message = "Failed to connect. Either the camera isn't enabled on the EOB or there is no connection";
                Buttons_Enabled = true;
                Close_Camera();
                return;
            }
        }

        private void On_Connected(object sender, EventArgs e)
        {
            Status_Message = "Waiting for EOB to accept request...";
            Buttons_Enabled = false;
        }

        private void On_Disconnected(object sender, EventArgs e)
        {
            Status_Message = "Server/client has disconnected";
            Close_Camera();
        }

        private void On_Server_Accepted(object sender, EventArgs e)
        {
            Status_Message = string.Empty;
        }

        private void On_Data_Recieved(object sender, Data_Event_Args e)
        {
            Remote_Cam = e.Data;
        }

        private async void Read_Local_Cam(object timer)
        {
            Local_Cam = _webcam_Handler.Read();
            await _network_Socket.Send(Local_Cam);
        }

        private void Close_Camera()
        {
            Dispose();
            Buttons_Enabled = true;
            Local_Cam = null;
            Remote_Cam = null;
        }

        private async Task Close_Connection()
        {
            Close_Camera();
            await _navigation_Service.Navigate<Eobs_ViewModel>();

            //leave network
            var status = await _zeroTier_Service.Get_Status();
            await _eob_Service.ZT_Node_Reset(Eob.Network_Id, status.address);
            await _zeroTier_Service.Leave_Network(Eob.Network_Id);
        }

        private async Task Scan_Ip_Addresses()
        {
            Ip_List = new MvxObservableCollection<string>();
            var tasks = new List<Task>();

            foreach (var route in Routes)
            {
                var range = Ip_Helper.Subnet_To_Range(route);

                for (uint i = range.Start; i <= range.End; i++)
                {
                    var ping = new Ping();
                    tasks.Add(Scan_Async(ping, Ip_Helper.Uint_To_String(i)));
                }
            }

            await Task.WhenAll(tasks);

            if (!Ip_List.Any())
                Ip_List.Add("None");
        }

        private async Task Scan_Async(Ping ping, string ip)
        {
            try
            {
                for (int i = 0; i < 5; i++)
                {
                    var reply = await ping.SendPingAsync(ip, 100);
                    if (reply.Status == IPStatus.Success)
                    {
                        // ignore certain ip addresses
                        string[] split = ip.Split('.');
                        if (int.Parse(split[3]) >= 249)
                            break;

                        Ip_List.Add(ip);
                        break;
                    }
                }
                ping.Dispose();
            }
            catch (Exception ex)
            {
            }
        }

        private async Task Add_Subnet()
        {
            string cidr = string.Join('/', Subnet, Mask);
            const string regex = @"^((\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.){3}(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$";

            if (string.IsNullOrEmpty(Subnet) ||
                string.IsNullOrEmpty(Mask) ||
                !Regex.IsMatch(Subnet, regex))
            {
                Status_Message = "Incorrect subnet format";
                return;
            }

            if (Routes.Any(x => x == cidr))
            {
                Status_Message = "Subnet already exists";
                return;
            }

            Routes.Add(cidr);

            var status = await _zeroTier_Service.Get_Status();

            await _eob_Service.ZT_Add_Subnet(Eob.Network_Id, Subnet, Mask);
            await _eob_Service.ZT_Node_Assign(Eob.Network_Id, status.address);

            Status_Message = string.Empty;
        }

        private async Task Delete_Subnet(string subnet)
        {
            if (subnet == "192.168.99.0/24")
            {
                Status_Message = "This subnet is not allowed to be removed.";
                return;
            }

            Routes.Remove(subnet);
            Subnet = string.Empty;
            Mask = string.Empty;

            string[] split = subnet.Split('/');
            var status = await _zeroTier_Service.Get_Status();

            await _eob_Service.ZT_Delete_Subnet(Eob.Network_Id, split[0], split[1]);
            await _eob_Service.ZT_Node_Reset(Eob.Network_Id, status.address);
            await _eob_Service.ZT_Node_Assign(Eob.Network_Id, status.address);
        }

        public override void ViewDisappearing()
        {
            Dispose();
            Menu.Menu_Open = true;
            Menu.Menu_Enabled = true;
        }

        public void Dispose()
        {
            _timer?.Dispose();
            _webcam_Handler?.Dispose();
            _network_Socket?.Dispose();
        }
    }
}
