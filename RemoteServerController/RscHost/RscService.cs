/******************************************************************************
 * Remote Server Controller, http://rsc.codeplex.com                          *
 *                                                                            *
 * Copyright (C) 2014 Karel Prajs                                             *
 *                                                                            *
 * This program is free software: you can redistribute it and/or modify       *
 * it under the terms of the GNU General Public License as published by       *
 * the Free Software Foundation, either version 3 of the License, or          *
 * (at your option) any later version.                                        *
 *                                                                            *
 * This program is distributed in the hope that it will be useful,            *
 * but WITHOUT ANY WARRANTY; without even the implied warranty of             *
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the              *
 * GNU General Public License for more details.                               *
 *                                                                            *
 * You should have received a copy of the GNU General Public License          *
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.      *
 ******************************************************************************/
namespace RscHost
{
    // System namespaces
    using System;
    using System.ComponentModel;
    using System.ServiceProcess;
    using System.ServiceModel.Web;
    using System.Configuration.Install;

    // Project namespaces
    using RscConfig;
    using RscLog;
    
    /// <summary>
    /// Host class which can be used both for standalone application host and Windows Managed Service host.
    /// The standalone mode is not recommended for production usage. It is mentioned only for development, debugging and tests.
    /// Only single WCF host is supported at the moment (do not mix up WCF host and application host).
    /// </summary>
    public partial class RscService : ServiceBase
    {
        WebServiceHost host = null;

        public RscService()
        {
            InitializeComponent();
        }

        #region Manual control for standalone host

        /// <summary>
        /// Start the program manually. Use only for standalone host mode.
        /// </summary>
        public void StartManually()
        {
            OnStart(new string[]{});
        }
        /// <summary>
        /// Stop the program manually. Use only for standalone host mode.
        /// </summary>
        public void StopManually()
        {
            OnStop();
        }

        #endregion

        #region Windows Managed Service interface

        protected override void OnStart(string[] args)
        {
            try
            {
                if (Configurator.Settings.Network.UseSSL)
                {
                    host = RscCore.Endpoints.GetRESTHostSSL();
                }
                else
                {
                    host = RscCore.Endpoints.GetRESTHost();
                }
                if (host != null)
                {
                    host.Open();
                    Log.Info("Service is running on " + Configurator.Settings.Network.Host + ":" + Configurator.Settings.Network.Port);
                }
                else
                {
                    Log.Error("Host not created.");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Host could not been started.");
                this.OnStop();
            }
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
                Log.Error(ex, "Host could not been closed correctly.");
            }
        }

        #endregion
    }

    /// <summary>
    /// Class allows easy installation of this application as Windows Managed Service.
    /// </summary>
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
            service.ServiceName = Constants.AppName;
            service.DisplayName = Constants.AppDisplayName;
            service.Description = "Service allows remote users to control this server.";
            service.StartType = ServiceStartMode.Automatic;
            Installers.Add(process);
            Installers.Add(service);
        }
    }
}
