using Engineers_App.Core.Services;
using Engineers_App.Core.Models;
using Engineers_App.Core.Models.ZeroTier;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utilities;

namespace Engineers_App.Core.View_Models
{
    public class Eobs_ViewModel : MvxViewModel
    {
        private readonly IEob_Service _eob_Service;
        private readonly IZeroTier_Service _zeroTier_Service;
        private readonly IMvxNavigationService _navigation_Service;
        private readonly ILogger<Eobs_ViewModel> _logger;

        public Eobs_ViewModel(
            IEob_Service eob_Api,
            IZeroTier_Service zeroTier_Api,
            IMvxNavigationService navigation_Service,
            ILogger<Eobs_ViewModel> logger)
        {
            _eob_Service = eob_Api;
            _zeroTier_Service = zeroTier_Api;
            _navigation_Service = navigation_Service;
            _logger = logger;
        }

        public override async Task Initialize()
        {
            await Refresh();
        }

        #region View properties

        public IMvxAsyncCommand Refresh_Command => new MvxAsyncCommand(Refresh);
        public IMvxAsyncCommand<Eob> Connect_Command => new MvxAsyncCommand<Eob>(x => Connect(x));

        private MvxObservableCollection<Eob> _eobs;
        public MvxObservableCollection<Eob> Eobs
        {
            get => _eobs;
            set => SetProperty(ref _eobs, value);
        }

        private string _status_Message;
        public string Status_Message
        {
            get => _status_Message;
            set => SetProperty(ref _status_Message, value);
        }

        #endregion

        private async Task Refresh()
        {
            var eobs = await _eob_Service.Get_All_Eobs();
            var tasks = new List<Task>();

            foreach (var eob in eobs)
            {
                tasks.Add(Task.Run(async() => 
                { 
                    eob.Node = await _eob_Service.ZT_Get_Node(eob.Network_Id, eob.Node_Id); 
                }));
            }

            await Task.WhenAll(tasks);
            Eobs = new MvxObservableCollection<Eob>(eobs);
        }

        private async Task Connect(Eob eob)
        {
            Status_Message = "Connecting...";

            var node = await _eob_Service.ZT_Get_Node(eob.Network_Id, eob.Node_Id);
            if (!node.online)
            {
                Status_Message = "The EOB you're trying to access is not online right now.";
                return;
            }

            var ret = await _zeroTier_Service.Join_Network(eob.Network_Id);
            if (ret == null)
                return;

            var network = new ZT_Network();
            for (int i = 0; i < 30; i++)
            {
                if (i == 30)
                {
                    Status_Message = "Failed to connect";
                    return;
                }

                network = await _zeroTier_Service.Get_Network(eob.Network_Id);

                if (network.status == "OK" || network.status == "ACCESS_DENIED")
                    break;

                await Task.Delay(200);
            }

            var status = await _zeroTier_Service.Get_Status();
            if (!await _eob_Service.ZT_Node_Assign(eob.Network_Id, status.address))
            {
                if (_eob_Service.Status_Code == HttpStatusCode.Forbidden)
                    Status_Message = "Network is full!";
                else
                    Status_Message = "Failed to connect";

                return;
            }

            // refresh incase of ACCESS_DENIED
            network = await _zeroTier_Service.Get_Network(eob.Network_Id);
            await _navigation_Service.Navigate<Personal_ViewModel, Personal_ViewModel_Parameters>(
                new Personal_ViewModel_Parameters { Eob = eob, Routes = network.routes });
        }
    }
}
