using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineers_App.Core.View_Models
{
    public class Settings_ViewModel : MvxViewModel
    {
        private readonly IConfiguration _config;
        private readonly ILogger<Settings_ViewModel> _logger;

        public Settings_ViewModel(IConfiguration config, ILogger<Settings_ViewModel> logger)
        {
            _config = config;
            _logger = logger;

            Is_Dark_Theme = _config.GetValue("Dark_Theme", true);
        }

        #region View properties

        private bool _is_Dark_Theme;
        public bool Is_Dark_Theme
        {
            get => _is_Dark_Theme;
            set => _is_Dark_Theme = value;
        }

        #endregion
    }
}
