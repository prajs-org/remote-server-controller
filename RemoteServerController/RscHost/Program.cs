﻿/******************************************************************************
 * Remote Server Controller                                                   *
 * https://github.com/prajs-org/remote-server-controller                      *
 * Copyright (C) 2014-2018 Karel Prajs, karel@prajs.org                        *
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
        /// Instance of the main service
        /// </summary>
        static RscService service;
        /// <summary>
        /// Flag whether we are in standalone mode (not windows service)
        /// </summary>
        static bool standAloneMode = false;
        /// <summary>
        /// The main entry point for the application.
        /// Application can be executed as Windows Managed Service or with standalone host.
        /// Standalone host is not recommended for production usage.
        /// Use argument configured in constant StandaloneHostFlag to run this application in standalone mode.
        /// </summary>
        static void Main(string[] args)
        {
            // Init logger
            RscLog.RscLog.Init(RscLog.RscLog.Logger.Log4Net, Constants.AppName);
            RscLog.RscLog.Info(String.Empty);
            RscLog.RscLog.Info("------------------------------------------------------------------------------------------");
            RscLog.RscLog.Info("Started on {0} {1}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString());
            RscLog.RscLog.Info("------------------------------------------------------------------------------------------");
            RscLog.RscLog.Info(String.Empty);

            // Create the service
            service = new RscService();

            // Handle unhandled exceptions in service
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            // Start service or run it on console for debug
            if (args.Count() > 0)
            {
                if (args[0] == Constants.StandaloneHostFlag)
                {
                    // ----------------------
                    // Run in standalone host
                    // ----------------------

                    standAloneMode = true;

                    // Print welcome message
                    Helpers.WelcomeMessage();
                    // Start service
                    service.StartManually();
                    // Wait on exit command
                    while (true)
                    {
                        string input = Console.ReadLine();
                        if (input == StaticConfiguration.Instance.General.QuitToken)
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

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e != null && e.ExceptionObject != null)
            {
                RscLog.RscLog.Error(e.ExceptionObject.ToString(), "Unhandled exception!");
            }
            if (standAloneMode)
            {
                RscLog.RscLog.Error(e.ExceptionObject.ToString());
                RscLog.RscLog.Error("Unhandled exception! Exit!");
                service.StopManually();
            }
            else
            {
                RscLog.RscLog.Error(e.ExceptionObject.ToString());
                RscLog.RscLog.Error("Unhandled exception! Going to stop the service!");
                ServiceController sc = new ServiceController(Constants.AppName);
                sc.Stop();
                sc.WaitForStatus(ServiceControllerStatus.Stopped);
                RscLog.RscLog.Error("Service stopped. Exit.");
            }
            Environment.Exit(1);
        }
    }
}
