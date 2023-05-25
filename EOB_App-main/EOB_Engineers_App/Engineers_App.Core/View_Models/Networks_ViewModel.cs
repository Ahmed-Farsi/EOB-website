using Engineers_App.Core.Services;
using Engineers_App.Core.Models.ZeroTier;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Engineers_App.Core.View_Models
{
    public class Networks_ViewModel : MvxViewModel
    {
        private readonly IZeroTier_Service _zerotier;
        private readonly ILogger<Networks_ViewModel> _logger;
        private object _lock = new object();

        public Networks_ViewModel(IZeroTier_Service zerotier, ILogger<Networks_ViewModel> logger)
        {
            _zerotier = zerotier;
            _logger = logger;

            Network_Join_Button = "Join";
        }

        public override async Task Initialize()
        {
            await Refresh();
        }

        #region View properties

        public IMvxAsyncCommand Join_Network_Command => new MvxAsyncCommand(Join_Network);
        public IMvxAsyncCommand<string> Leave_Network_Command => new MvxAsyncCommand<string>(x => Leave_Network(x));
        public IMvxAsyncCommand Refresh_Command => new MvxAsyncCommand(Refresh);

        private MvxObservableCollection<ZT_Network> _networks;
        public MvxObservableCollection<ZT_Network> Networks
        {
            get
            {
                lock (_lock)
                { 
                    return _networks;
                }
            }
            set
            {
                lock (_lock)
                { 
                    SetProperty(ref _networks, value);
                }
            }
        }

        private string _network_Id;
        public string Network_Id
        {
            get => _network_Id;
            set => SetProperty(ref _network_Id, value);
        }

        private string _network_Error_Message;
        public string Network_Error_Message
        {
            get => _network_Error_Message;
            set => SetProperty(ref _network_Error_Message, value);
        }

        private string _network_Join_Button;
        public string Network_Join_Button
        {
            get => _network_Join_Button;
            set => SetProperty(ref _network_Join_Button, value);
        }

        #endregion

        private async Task Join_Network()
        {
            Network_Error_Message = string.Empty;

            if (string.IsNullOrEmpty(Network_Id) || Network_Id.Length < 16)
            {
                Network_Error_Message = "Network ID has to be 16 characters";
                return;
            }

            if (!long.TryParse(Network_Id, NumberStyles.HexNumber, null, out _))
            {
                Network_Error_Message = "Network ID is not valid";
                return;
            }

            if (Networks.Any(x => x.id == Network_Id))
            {
                Network_Error_Message = "Network already exists";
                return;
            }

            Network_Join_Button = "Joining...";

            await _zerotier.Join_Network(Network_Id);
            await Task.Delay(1000); // wait an extra second to bypass REQUESTING status
            await Refresh();

            Network_Join_Button = "Join";
        }

        private async Task Leave_Network(string network_Id)
        {
            if (await _zerotier.Leave_Network(network_Id))
            {
                Networks.Remove(Networks
                    .Where(x => x.id == network_Id)
                    .FirstOrDefault());
            }
        }

        private async Task Refresh()
        {
            var networks = await _zerotier.Get_Networks();
            Networks = new MvxObservableCollection<ZT_Network>(networks);
        }
    }
}
