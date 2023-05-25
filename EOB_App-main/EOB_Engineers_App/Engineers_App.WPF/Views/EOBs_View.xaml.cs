using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Platforms.Wpf.Views;

using System;
using System.Collections.Generic;
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
    /// Interaction logic for Eobs_View.xaml
    /// </summary>

    [MvxContentPresentation(StackNavigation = false)]
    public partial class Eobs_View : MvxWpfView
    {
        public Eobs_View()
        {
            InitializeComponent();
        }
    }
}
