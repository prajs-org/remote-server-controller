using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Description;

using RscConfig;
using RscLog;
using RscCore;

namespace RscHost
{
    internal static class Helpers
    {
        internal static void WelcomeMessage()
        {
            var copyYear = DateTime.Now.Year == 2014 ? "2014" : "2014-" + DateTime.Now.Year;
            // Print some information for user
            Console.WriteLine("Remote Server Controller");
            Console.WriteLine("Copyright (C) " + copyYear + " Karel Prajs");
            Console.WriteLine("http://rsc.codeplex.com");
            Console.WriteLine();
            Console.WriteLine("LICENSE:");
            Console.WriteLine(new StringBuilder("This program is free software: you can redistribute it and/or modify")
                .Append(Environment.NewLine)
                .AppendLine("it under the terms of the GNU General Public License as published by")
                .AppendLine("the Free Software Foundation, either version 3 of the License, or")
                .AppendLine("(at your option) any later version.").ToString());
            Console.WriteLine(String.Format("Listening on {0}:{1}", Configurator.Settings.Network.Host, Configurator.Settings.Network.Port));
            Console.WriteLine();
            Console.WriteLine(String.Format("Type <{0}> to stop listening and close program.", Configurator.Settings.GeneralSettings.QuitToken));
        }

        internal static void SetLogLevel()
        {
            switch(Configurator.Settings.GeneralSettings.LogLevel)
            {
                case "Silent":
                    Log.Level = Log.LogLevel.Silent;
                    break;
                case "Debug":
                    Log.Level = Log.LogLevel.Debug;
                    break;
                case "Info":
                    Log.Level = Log.LogLevel.Info;
                    break;
                case "Warning":
                    Log.Level = Log.LogLevel.Warning;
                    break;
                case "Error":
                    Log.Level = Log.LogLevel.Error;
                    break;
                case "Fatal":
                    Log.Level = Log.LogLevel.Fatal;
                    break;
                default:
                    Log.Level = Log.LogLevel.Debug;
                    Log.Debug("Unknown LogLevel<" + Configurator.Settings.GeneralSettings.LogLevel + ">, using Debug.");
                    break;
            };
        }

        internal static void CreateHTTPHost(out WebServiceHost host)
        {
            // Create host
            host = new WebServiceHost(typeof(RESTController));
            // No security
            WebHttpBinding binding = new WebHttpBinding(WebHttpSecurityMode.None);
            // Allow cross domain scripts
            binding.CrossDomainScriptAccessEnabled = Configurator.Settings.Network.CrossDomainScriptAccessEnabled;
            // Create endpoint
            ServiceEndpoint endPoint = new ServiceEndpoint(ContractDescription.GetContract(typeof(RESTController)),
                                                           binding,
                                                           new EndpointAddress(new Uri(
                                                               new StringBuilder("http://")
                                                               .Append(Configurator.Settings.Network.Host)
                                                               .Append(":")
                                                               .Append(Configurator.Settings.Network.Port)
                                                               .Append("/RESTController")
                                                               .ToString())));
            // Return final host
            host.AddServiceEndpoint(endPoint);
        }
    }
}
