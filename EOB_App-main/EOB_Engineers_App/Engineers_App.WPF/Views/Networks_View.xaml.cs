using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Platforms.Wpf.Views;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Engineers_App.WPF.Views
{
    /// <summary>
    /// Interaction logic for Networks_View.xaml
    /// </summary>

    [MvxContentPresentation(StackNavigation = false)]
    public partial class Networks_View : MvxWpfView
    {
        public Networks_View()
        {
            InitializeComponent();
        }
    }
}
