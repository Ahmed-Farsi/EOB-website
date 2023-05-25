using MaterialDesignThemes.Wpf;

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
    /// Interaction logic for Settings_View.xaml
    /// </summary>
    [MvxContentPresentation(StackNavigation = false)]
    public partial class Settings_View : MvxWpfView
    {
        public Settings_View()
        {
            InitializeComponent();
            var paletteHelper = new PaletteHelper();
            var theme = paletteHelper.GetTheme();
            Is_Dark_Button.IsChecked = theme.GetBaseTheme().ToString() == "Dark";

        }

        private void Is_Dark_Button_Click(object sender, RoutedEventArgs e)
                    => ModifyTheme(Is_Dark_Button.IsChecked == true);

        private static void ModifyTheme(bool isDarkTheme)
        {
            var paletteHelper = new PaletteHelper();
            var theme = paletteHelper.GetTheme();

            theme.SetBaseTheme(isDarkTheme ? Theme.Dark : Theme.Light);
            paletteHelper.SetTheme(theme);
        }
    }
}
