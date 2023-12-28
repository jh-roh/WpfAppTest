using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;

namespace WpfAppTest
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        Mutex mutex;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            string mutexName = "회사명.고객관리";

            bool bCreateNew;

            mutex = new Mutex(true,mutexName, out bCreateNew);

            if(!bCreateNew)
            {
                Shutdown();
            }



        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Debug.WriteLine("Application_Startup");
        }

        private void Application_Activated(object sender, EventArgs e)
        {
            Debug.WriteLine("Application_Activated");

        }

        private void Application_Deactivated(object sender, EventArgs e)
        {
            Debug.WriteLine("Application_Deactivated");

        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Debug.WriteLine("Application_Exit");
            e.ApplicationExitCode = -1;

        }
    }
}
