using Engineers_App.Core.Services;
using Engineers_App.Core.View_Models;
using MvvmCross;
using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
namespace Engineers_App.WPF.Views
{
    /// <summary>
    /// Interaction logic for Main_View.xaml
    /// </summary>
    [MvxWindowPresentation(Identifier = nameof(Main_ViewModel), Modal = false)]
    [MvxViewFor(typeof(Main_ViewModel))]
    public partial class Main_View : MvxWindow
    {
        public Main_View()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((dynamic)this.DataContext).Password = ((PasswordBox)sender).Password; }
        }
    }
}
