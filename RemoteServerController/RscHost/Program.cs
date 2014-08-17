using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

using RscConfig;

namespace RscHost
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            // Create the service
            var service = new RscService();

            // Start service or run it on console for debug
            if (args.Count() > 0 && args[0] == "/CONSOLE")
            {
                // Print welcome message
                Helpers.WelcomeMessage();

                // Start service
                service.StartForDebug(new string[] { });

                // Wait on exit command
                while (true)
                {
                    string input = Console.ReadLine();
                    if (input == Configurator.Settings.GeneralSettings.QuitToken)
                        break;
                }
                
                // Stop service
                service.StopForDebug();
            }
            else
            {
                ServiceBase.Run(service);
            }
        }
    }
}
