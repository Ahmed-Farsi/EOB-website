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
    public class Home_ViewModel : MvxViewModel
    {
        private readonly ILogger<Home_ViewModel> _logger;

        public Home_ViewModel(ILogger<Home_ViewModel> logger)
        {
            _logger = logger;
        }


        #region View properties

        #endregion
    }
}
