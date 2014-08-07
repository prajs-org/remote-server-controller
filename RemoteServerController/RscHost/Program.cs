/*
Copyright (C) 2014 Karel Prajs
This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.ServiceProcess;
using System.Diagnostics;
using System.Text;

using RscCore;
using RscLog;
using RscConfig;
using System.ComponentModel;
using System.Configuration.Install;

namespace RscHost
{
    class RscHost : ServiceBase
    {
        // Visible name of program
        public const string ProgramName = "Remote Server Controller";

        // Service
        WebServiceHost host = null;

        static void Main() { }

        protected override void OnStart(string[] args)
        {
            // Init logger
            Log.Init(ProgramName);
            // Set log level
            Helpers.SetLogLevel();

            try
            {
                // Close the service if already running
                if (host != null)
                {
                    host.Close();
                    host = null;
                }
                // Create and open host
                if (Configurator.Settings.Network.UseSSL)
                {
                    throw new NotImplementedException("SSL is not implemented yet.");
                }
                else
                {
                    Helpers.CreateHTTPHost(out host);
                }
                host.Open();
            }
            catch (Exception ex)
            {
                Log.Error("General error: " + ex.Message);
                if (ex.InnerException != null)
                    Log.Error("General error: " + ex.InnerException.Message);
            }
        }

        protected override void OnStop()
        {
            if (host != null)
            {
                host.Close();
                host = null;
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
            service.ServiceName = RscHost.ProgramName;
            Installers.Add(process);
            Installers.Add(service);
        }
    }
}
