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
using System.Diagnostics;
using System.Text;

using RscCore;
using RscLog;
using RscConfig;

namespace RscHost
{
    class RscHost
    {
        static void Main(string[] args)
        {
            // Visible name of program
            const string programName = "Remote Server Controller";

            // Set window title
            Console.Title = programName;

            WebServiceHost host = null;

            try
            {
                // Set log level
                Helpers.SetLogLevel();

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

                // Print welcome message
                Helpers.WelcomeMessage(programName);

                // Wait on exit command
                while (true)
                {
                    string input = Console.ReadLine();
                    if (input == Configurator.Settings.GeneralSettings.QuitToken)
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Fatal("General error");
                Log.Fatal(ex.Message);
                if (ex.InnerException != null)
                    Log.Fatal(ex.InnerException.Message);
                Log.Fatal("Press ENTER to exit.");
                Console.Read();
            }
            finally
            {
                // Close endpoint
                try
                {
                    if (host != null)
                        host.Close();
                }
                catch { }
            }
        }
    }
}
