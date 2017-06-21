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

namespace RscConfig
{
    // System namespaces
    // -- none

    // Project namespaces
    // -- none

    /// <summary>
    /// Storage of all constants which are not mentioned to be configurable but should not been
    /// directly in code as "magic numbers". This class should provide as little constants as possible.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Flag used to force standalone host.
        /// Should be set as Command line argument in host project properties.
        /// </summary>
        public static string StandaloneHostFlag = "/STANDALONE";
        /// <summary>
        /// Flag used during installation to install Windows Managed Service.
        /// </summary>
        public static string InstallServiceFlag = "/INSTALLSERVICE";
        /// <summary>
        /// Flag used during installation to uninstall Windows Managed Service.
        /// </summary>
        public static string UninstallServiceFlag = "/UNINSTALLSERVICE";
        /// <summary>
        /// Flag used during installation to reinstall Windows Managed Service.
        /// </summary>
        public static string ReinstallServiceFlag = "/REINSTALLSERVICE";
        /// <summary>
        /// Use in various cases as common timeout
        /// </summary>
        public static int GeneralTimeout = 5000;
        /// <summary>
        /// Shortcut of full name of this application
        /// </summary>
        public static string AppShortcut = "RSC";
        /// <summary>
        /// Full name of this applicaton as single word (without spaces).
        /// Should be same as Assembly Name because of issues with Windows Managed Service.
        /// </summary>
        public static string AppName = "RemoteServerController";
        /// <summary>
        /// Full name of this application in "Display" format (what should user see)
        /// </summary>
        public static string AppDisplayName = "Remote Server Controller";
        /// <summary>
        /// Description of uknown status
        /// </summary>
        public static string UknownToken = "Unknown";
        /// <summary>
        /// Token will be written to trace when ALL service statuses are requested (instead of service name).
        /// </summary>
        public static string AllItems = "ALL";
        /// <summary>
        /// Extension of file with configuration
        /// </summary>
        public static string ConfigFileExtension = ".config";
        /// <summary>
        /// How many times should I try to reload config from config file.
        /// </summary>
        public const int ConfigReloadRetry = 0;
    }
}
