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
    using System.Linq;
    using System.ServiceProcess;

    // Project namespaces
    using RscConfig;
    using System.Configuration.Install;
    using System.Reflection;

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// Application can be executed as Windows Managed Service or with standalone host.
        /// Standalone host is not recommended for production usage.
        /// Use argument configured in constant StandaloneHostFlag to run this application in standalone mode.
        /// </summary>
        static void Main(string[] args)
        {
            // Create the service
            var service = new RscService();

            // Start service or run it on console for debug
            if (args.Count() > 0)
            {
                if (args[0] == Constants.StandaloneHostFlag)
                {
                    // ----------------------
                    // Run in standalone host
                    // ----------------------

                    // Print welcome message
                    Helpers.WelcomeMessage();
                    // Start service
                    service.StartManually();
                    // Wait on exit command
                    while (true)
                    {
                        string input = Console.ReadLine();
                        if (input == Configurator.Settings.GeneralSettings.QuitToken)
                            break;
                    }
                    // Stop service
                    service.StopManually();
                }
                else if (args[0] == Constants.InstallServiceFlag)
                {
                    // --------------------------------------------------------
                    // Install this program as Windows Managed Service and quit
                    // --------------------------------------------------------

                    ServiceHandler.InstallService(Constants.AppName);
                    return;
                }
                else if (args[0] == Constants.UninstallServiceFlag)
                {
                    // ----------------------------------------------------------
                    // Uninstall this program as Windows Managed Service and quit
                    // ----------------------------------------------------------

                    ServiceHandler.UninstallService(Constants.AppName);
                    return;
                }
                else if (args[0] == Constants.ReinstallServiceFlag)
                {
                    // ----------------------------------------------------------
                    // Reinstall this program as Windows Managed Service and quit
                    // ----------------------------------------------------------

                    ServiceHandler.UninstallService(Constants.AppName);
                    ServiceHandler.InstallService(Constants.AppName);
                    return;
                }
            }
            else
            {
                // ------------------------------
                // Run as Windows Managed Service
                // ------------------------------

                ServiceBase.Run(service);
            }
        }
    }
}
