/******************************************************************************
 * Remote Server Controller                                                   *
 * https://github.com/prajs-org/remote-server-controller                      *
 * Copyright (C) 2014-2017 Karel Prajs, karel@prajs.org                        *
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
    using System.Configuration.Install;
    using System.Reflection;
    using System.Windows.Forms;
    using System.ServiceProcess;

    // Project namespaces
    // -- none

    /// <summary>
    /// Handler for (un)installing Windows Managed Service via Installer.
    /// </summary>
    internal static class ServiceHandler
    {
        /// <summary>
        /// Install and start service.
        /// </summary>
        /// <param name="serviceName">Service name</param>
        internal static void InstallService(string serviceName)
        {
            try
            {
                ManagedInstallerClass.InstallHelper(new string[] { Assembly.GetExecutingAssembly().Location });
                StartService(serviceName);
            }
            catch
            {
                try
                {
                    ManagedInstallerClass.InstallHelper(new string[] { "/u", Assembly.GetExecutingAssembly().Location });
                    ManagedInstallerClass.InstallHelper(new string[] { Assembly.GetExecutingAssembly().Location });
                    StartService(serviceName);
                }
                catch
                {
                    MessageBox.Show("Could not install Windows Managed Service. Try again.");
                }
            }
        }
        /// <summary>
        /// Stop and uninstall service.
        /// </summary>
        /// <param name="serviceName">Service name</param>
        internal static void UninstallService(string serviceName)
        {
            try
            {
                StopService(serviceName);
                ManagedInstallerClass.InstallHelper(new string[] { "/u", Assembly.GetExecutingAssembly().Location });
            }
            catch { }
        }
        /// <summary>
        /// Start installed service.
        /// </summary>
        /// <param name="serviceName">Service name</param>
        private static void StartService(string serviceName)
        {
            ServiceController service = new ServiceController(serviceName);
            if (service.Status != ServiceControllerStatus.Running)
                service.Start();
        }
        /// <summary>
        /// Stop installed service.
        /// </summary>
        /// <param name="serviceName">Service name</param>
        private static void StopService(string serviceName)
        {
            ServiceController service = new ServiceController(serviceName);
            if (service.Status != ServiceControllerStatus.Stopped)
                service.Stop();
        }
    }
}
