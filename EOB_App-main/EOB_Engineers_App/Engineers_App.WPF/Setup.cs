using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;
using MvvmCross.Platforms.Wpf.Core;
using MvvmCross.Platforms.Wpf.Presenters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Engineers_App.WPF
{
    public class Setup : MvxWpfSetup<Core.App>
    {
        protected override ILoggerProvider CreateLogProvider()
        {
            return new EventLogLoggerProvider();
        }

        protected override ILoggerFactory CreateLogFactory()
        {
            return new LoggerFactory();
        }
    }   
}
