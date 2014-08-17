using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.ServiceModel.Web;

using RscConfig;
using RscLog;
using System.Configuration.Install;

namespace RscHost
{
    public partial class RscService : ServiceBase
    {
        WebServiceHost host = null;

        public RscService()
        {
            InitializeComponent();
        }

        public void StartForDebug(string[] args)
        {
            OnStart(args);
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                // Create and open host
                if (Configurator.Settings.Network.UseSSL)
                {
                    host = RscCore.Endpoints.GetRESTHostSSL();
                }
                else
                {
                    host = RscCore.Endpoints.GetRESTHost();
                }
                host.Open();
                Log.Info("Service is running on " + Configurator.Settings.Network.Host + ":" + Configurator.Settings.Network.Port);
            }
            catch (Exception ex)
            {
                Log.Error("General error: " + ex.Message);
                if (ex.InnerException != null)
                {
                    Log.Error(ex.InnerException.Message);
                }
                this.OnStop();
            }
        }

        public void StopForDebug()
        {
            OnStop();
        }

        protected override void OnStop()
        {
            try
            {
                if (host != null)
                    host.Close();
            }
            catch (Exception ex)
            {
                Log.Error("Host could not been closed: " + ex.Message);
            }
        }
    }

    [RunInstaller(true)]
    public class ProjectInstaller : Installer
    {
        private ServiceProcessInstaller process;
        private ServiceInstaller service;

        public ProjectInstaller()
        {
            process = new ServiceProcessInstaller();
            process.Account = ServiceAccount.LocalSystem;
            service = new ServiceInstaller();
            service.ServiceName = "RscService";
            service.DisplayName = "Remote Server Controller";
            service.Description = "Service allows remote users to control this server.";
            Installers.Add(process);
            Installers.Add(service);
        }
    }
}
