using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineers_App.Core.View_Properties
{
    public class Menu : MvxNotifyPropertyChanged
    {
        private bool _menu_Open;
        public bool Menu_Open
        {
            get => _menu_Open;
            set => SetProperty(ref _menu_Open, value);
        }

        private bool _menu_Enabled;
        public bool Menu_Enabled
        {
            get => _menu_Enabled;
            set => SetProperty(ref _menu_Enabled, value);
        }
    }
}
