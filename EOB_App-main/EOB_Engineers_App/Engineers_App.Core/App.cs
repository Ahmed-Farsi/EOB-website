using AutoMapper;
using Engineers_App.Core.Services;
using Engineers_App.Core.Models;
using Engineers_App.Core.View_Properties;
using Engineers_App.Core.View_Models;
using Microsoft.Extensions.Configuration;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace Engineers_App.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            Add_Configuration();
            Configure_MvvmCross();
            Configure_ZeroTier();
        }

        private void Add_Configuration()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
#if DEBUG
            builder.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
#else
            builder.AddJsonFile("appsettings.Production.json", optional: true, reloadOnChange: true);
#endif

            var configuration = builder.Build();

            Mvx.IoCProvider.RegisterSingleton<IConfiguration>(configuration);
        }

        private void Configure_MvvmCross()
        {
            Mvx.IoCProvider.ConstructAndRegisterSingleton<IZeroTier_Service, ZeroTier_Service>();
            Mvx.IoCProvider.ConstructAndRegisterSingleton<IEob_Service, Eob_Service>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton(() => new Menu());

            RegisterAppStart<Main_ViewModel>();
        }

        private void Configure_ZeroTier()
        {
            var Zerotier = Mvx.IoCProvider.Resolve<IZeroTier_Service>();

            string authTokenPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                @"EOB\");

            Zerotier.Add_Token(authTokenPath);
        }
    }
}
