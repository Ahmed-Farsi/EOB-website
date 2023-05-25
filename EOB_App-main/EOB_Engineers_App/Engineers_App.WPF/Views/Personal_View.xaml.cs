using Engineers_App.Core.Services;
using Engineers_App.Core.View_Models;
using MvvmCross;
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
    /// Interaction logic for Personal_View.xaml
    /// </summary>

    [MvxContentPresentation(StackNavigation = false)]
    public partial class Personal_View : MvxWpfView 
    {
        public Personal_View()
        {
            InitializeComponent();
            Loaded += On_Loaded;
        }

        private void On_Loaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.Closing += On_Closing;
        }

        // Not the most elegant solution but it works.
        private void On_Closing(object sender, CancelEventArgs e)
        {
            if (ViewModel is IDisposable)
                (ViewModel as IDisposable).Dispose();

            var zeroTier_Service = Mvx.IoCProvider.Resolve<IZeroTier_Service>();
            var eob_Service = Mvx.IoCProvider.Resolve<IEob_Service>();
            var personal = (Personal_ViewModel)ViewModel;

            Task.Run(async() =>
            {
                var status = await zeroTier_Service.Get_Status();
                await eob_Service.ZT_Node_Reset(personal.Eob.Network_Id, status.address);
                await zeroTier_Service.Leave_Network(personal.Eob.Network_Id);
            });
        }
    }
}
