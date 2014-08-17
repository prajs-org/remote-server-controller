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
    using System.Text;

    // Project namespaces
    using RscConfig;

    /// <summary>
    /// Static class for helping functions.
    /// Should contain as little functions as possible - only when you really cannot put it elsewhere.
    /// </summary>
    internal static class Helpers
    {
        /// <summary>
        /// Print welcome message to Console in standalone mode.
        /// </summary>
        internal static void WelcomeMessage()
        {
            // Copyright year
            var copyYear = DateTime.Now.Year == 2014 ? "2014" : "2014-" + DateTime.Now.Year;

            string message = String.Format(@"{0}
Copyright (C) {1} Karel Prajs
http://rsc.codeplex.com

LICENSE:
This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Listening on {2}:{3}

NOTE: Application is running in standalone mode. This mode is not recommended
for production usage. Use Windows Managed Service for production mode.

Type {4} to stop listening and close program.",
                Constants.AppDisplayName,
                copyYear,
                Configurator.Settings.Network.Host,
                Configurator.Settings.Network.Port,
                Configurator.Settings.GeneralSettings.QuitToken);

            Console.WriteLine(message);
        }
    }
}
