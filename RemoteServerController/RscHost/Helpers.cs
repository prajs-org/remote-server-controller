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
            Console.WriteLine(Constants.AppDisplayName);
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
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("!!! This CONSOLE mode should be used only for debug.                         !!!");
            Console.WriteLine("!!! Do not use it for production. See documentation for details.             !!!");
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine();
            Console.WriteLine(String.Format("Type <{0}> to stop listening and close program.", Configurator.Settings.GeneralSettings.QuitToken));
        }
    }
}
