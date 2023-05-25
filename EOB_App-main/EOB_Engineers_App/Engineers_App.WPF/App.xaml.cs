using MvvmCross.Core;
using MvvmCross.Platforms.Wpf.Views;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Engineers_App.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : MvxApplication
    {
        protected override void RegisterSetup()
        {
            string process = Process.GetCurrentProcess().ProcessName;

            if (Process.GetProcesses().Count(p => p.ProcessName == process) > 1)
            { 
                MessageBox.Show(
                    "Instance already running",
                    "Warning",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);

                Environment.Exit(-1);
            }

            this.RegisterSetupType<Setup>();
        }
    }
}
