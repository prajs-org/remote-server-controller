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

// System namespaces
using System;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;

// Project namespaces
// -- none

namespace RscConfig
{
    public class ConfigWatcher
    {
        /// <summary>
        /// Informs anyone who cares, that the config file was changed.
        /// </summary>
        public event EventHandler Changed;

        private FileSystemWatcher watcher;

        private static ConfigWatcher instance;

        /// <summary>
        /// Get instance of config file watcher. When config file is changed, allowed sections are reloaded from file.
        /// </summary>
        /// <returns>Instanace of config file watcher.</returns>
        public static ConfigWatcher Instance()
        {
            if (instance == null)
                instance = new ConfigWatcher();
            return instance;
        }

        private ConfigWatcher()
        {
            watcher = new FileSystemWatcher();
            watcher.Path = AppDomain.CurrentDomain.BaseDirectory;
            watcher.Filter = AppDomain.CurrentDomain.FriendlyName + Constants.ConfigFileExtension;
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.IncludeSubdirectories = false;
            watcher.Changed += Watcher_Changed;
        }

        public void Start()
        {
            watcher.EnableRaisingEvents = true;
        }

        public void Stop()
        {
            watcher.EnableRaisingEvents = false;
        }

        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            DynamicConfiguration.RefreshFromFile();
            if (Changed != null)
            {
                Changed(this, new EventArgs());
            }
        }
    }
}
