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
namespace RscCore.Security
{
    // System namespaces
    using System;
    using System.Collections.Generic;

    // Project namespaces
    using RscConfig;
    using RscLog;

    internal class APIKeyManager
    {
        /// <summary>
        /// Singleton instance of this class.
        /// </summary>
        private static APIKeyManager instance = null;
        /// <summary>
        /// List of all allowed API keys.
        /// Dictionary is used to have a direct access via hash.
        /// So the Key contains API key, the Value is not used.
        /// The Value can be used for detailed information in future.
        /// </summary>
        Dictionary<string, bool> allowedAPIKeys = new Dictionary<string, bool>();
        /// <summary>
        /// Private constructor
        /// </summary>
        private APIKeyManager()
        {
            try
            {
                LoadAPIKeys();
            }
            catch (Exception ex)
            {
                RscLog.Error(ex, "Could not load API Keys.");
                allowedAPIKeys.Clear();
            }
        }
        /// <summary>
        /// Get instance of APIKeyManager.
        /// Class is used for checking if given API key is valid and allowed for processing.
        /// Implemented as singleton.
        /// </summary>
        public static APIKeyManager Instance()
        {
            if (instance == null)
                instance = new APIKeyManager();
            return instance;
        }
        /// <summary>
        /// Check if API Key is valid and allowed. 
        /// </summary>
        /// <param name="apiKey">API Key for check</param>
        /// <returns>True if API Key is valid</returns>
        public bool IsValidAPIKey(string apiKey)
        {
            return apiKey == null ? false : allowedAPIKeys.ContainsKey(apiKey);
        }
        /// <summary>
        /// Clear the list of API kyes and load them again.
        /// Loads from config file at the moment.
        /// Can be modified for any storage in future.
        /// </summary>
        /// <returns></returns>
        private void LoadAPIKeys()
        {
            allowedAPIKeys.Clear();
            foreach (var item in Configurator.Settings.Security.AllowedAPIKeys)
            {
                AddAPIKey key = (AddAPIKey)item;
                if (false == allowedAPIKeys.ContainsKey(key.Value))
                {
                    allowedAPIKeys.Add(key.Value, true);
                }
            }
        }
    }
}
