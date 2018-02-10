/******************************************************************************
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
        /// Key is name of security profile.
        /// Value uses Dictionary to have direct access to apiKeys, where key is apiKey and value is not used.
        /// </summary>
        Dictionary<string, Dictionary<string, int>> allowedAPIKeys = new Dictionary<string, Dictionary<string, int>>();
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
        public static APIKeyManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new APIKeyManager();
                return instance;
            }            
        }
        /// <summary>
        /// Check if API Key is valid and allowed. 
        /// </summary>
        /// <param name="securityProfile">Security profile</param>
        /// <param name="apiKey">API Key for check</param>
        /// <returns>True if API Key is valid</returns>
        public bool IsValidAPIKey(string securityProfile, string apiKey)
        {
            if(allowedAPIKeys.ContainsKey(securityProfile))
            {
                return allowedAPIKeys[securityProfile].ContainsKey(apiKey);
            }
            return false;
        }
        /// <summary>
        /// Clear the list of API kyes and load them again.
        /// Loads from config file at the moment.
        /// In case of any error (for example duplicates) just clear the list and load nothing.
        /// </summary>
        /// <returns></returns>
        private void LoadAPIKeys()
        {
            try
            {
                allowedAPIKeys.Clear();
                foreach (SecurityProfile securityProfile in DynamicConfiguration.Instance.Security.SecurityProfileCollection)
                {
                    allowedAPIKeys.Add(securityProfile.Name, new Dictionary<string, int>());
                    foreach (AllowedAPIKey apiKey in securityProfile.AllowedAPIKeyCollection)
                    {
                        allowedAPIKeys[securityProfile.Name].Add(apiKey.Value, 0);
                    }
                }
            }
            catch (Exception ex)
            {
                RscLog.Error(ex, "Could not load API Keys.");
                allowedAPIKeys.Clear();
            }
        }
    }
}
