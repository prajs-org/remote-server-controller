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
using RscCore;
using System.Text;
using RscLog;
using System.Diagnostics;

namespace RscCMDHost
{
    class RscCMD
    {
        // TODO: temporary, make it better
        static void Main(string[] args)
        {
            // Some basic configuration
            // TODO: make it real
            string cHost = "localhost";
            string cPort = "55001";
            string cQuitToken = "quit!";
            string cConsoleTitle = "Remote Service Controller";

            // Set window title
            Console.Title = cConsoleTitle;

            // Set log level
            Log.Level = Log.LogLevel.Info;

            WebServiceHost host = null;
            try
            {

                // Setup basic endpoint - unsafe, pure HTTP
                host = new WebServiceHost(typeof(RESTController));
                WebHttpBinding binding = new WebHttpBinding(WebHttpSecurityMode.None);
                binding.CrossDomainScriptAccessEnabled = true;
                ServiceEndpoint endPoint = new ServiceEndpoint(ContractDescription.GetContract(typeof(RESTController)),
                                                               binding,
                                                               new EndpointAddress(new Uri("http://" + cHost + ":" + cPort + "/RESTController")));
                host.AddServiceEndpoint(endPoint);
                host.Open();

                // Print some information for user
                Console.WriteLine("Remote Service Controller");
                Console.WriteLine("Copyright (C) 2014 Karel Prajs");
                Console.WriteLine();
                Console.WriteLine(new StringBuilder("This program is free software: you can redistribute it and/or modify")
                    .Append(Environment.NewLine)
                    .AppendLine("it under the terms of the GNU General Public License as published by")
                    .AppendLine("the Free Software Foundation, either version 3 of the License, or")
                    .AppendLine("(at your option) any later version."));
                Console.WriteLine();
                Console.WriteLine(String.Format("Listening on {0}:{1}", cHost, cPort));
                Console.WriteLine(String.Format("Type <{0}> to stop listening and close program.", cQuitToken));

                // Wait on exit command
                while (true)
                {
                    string input = Console.ReadLine();
                    if (input == cQuitToken)
                        break;
                }
            }
            catch(Exception ex)
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
                if (host != null)
                    host.Close();
            }
        }
    }
}
