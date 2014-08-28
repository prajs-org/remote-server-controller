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
namespace RscCore.Controllers
{
    // System namespaces
    using System;
    using System.Reflection;

    // Projet namespaces
    using RscConfig;
    using RscLog;
    using RscCore.Security;

    /// <summary>
    /// Static class designated for secure construction of controller objects.
    /// Each controller has to be constructed by this factory.
    /// Factory creates an object and set all necessary permissions according to configuration file.
    /// No controller can be created directly (by constructor) because such object would have no permissions.
    /// </summary>
    public static class ControlFactory
    {
        /// <summary>
        /// Create instance of service with configured permissions. The permissions are read-only in final object.
        /// Object may be created only if it is configured as allowed service in configuration file.
        /// </summary>
        /// <param name="serviceName">Name of service</param>
        /// <returns>Instance of Service or null if service is not specified in configuration file.</returns>
        public static Service GetService(string serviceName, string apiKey)
        {
            // This object will be returned if all succeeded
            Service service = null;
            // Configuration of service
            AddService serviceConfiguration = null;
            // If this variable is set to TRUE, no permissions will be set.
            bool forbidAll = false;

            // ---------------------------------------------------------
            // SECURITY NOTICE:
            // All mandatory checks (like API key, IP address...) should be done within following
            // try-catch block. If anything fail, the forbidAll flag has to be set to TRUE.

            try
            {
                // Prepare object with no permissions
                service = new Service(serviceName);

                // Load configuration of service from configuration file
                if (false == Configurator.Settings.Services.AllowedServices.GetService(serviceName, out serviceConfiguration))
                {
                    // Service is not configured and cannot be processed
                    Log.Alert("Processing of service<{0}> is not allowed because it is not configured!", serviceName);
                    forbidAll = true;
                }
                // Check API Key
                else if (Configurator.Settings.Security.CheckAPIKey)
                {
                    if (false == APIKeyManager.Instance().IsValidAPIKey(apiKey))
                    {
                        // Given API Key is not valid, request cannot be processed
                        Log.Alert("Processing of service<{0}> is not allowed because of invalid APIKey<{1}>!", serviceName, apiKey);
                        forbidAll = true;
                    }
                }
            }
            catch (Exception ex)
            {
                forbidAll = true;
                Log.Alert(ex, "Permissions could not been processed correctly. Forbidding all permissions.");
            }

            // ---------------------------------------------------------
            // SECURITY NOTICE:
            // The forbidAll flag is set now, so if it is still FALSE, you can start to allow some permissions.
            // Put your own checks inside the following if block.

            if (service != null && serviceConfiguration != null && forbidAll == false)
            {

                /*** HERE YOU CAN START TO ALLOWING COMMON PERMISSIONS ***/

                // --- Allow Start (service may be started)
                service.GetType().GetField("allowStart", BindingFlags.NonPublic | BindingFlags.Instance)
                    .SetValue(service, serviceConfiguration.AllowStart);

                // --- Allow Stop (service may be stopped)
                service.GetType().GetField("allowStop", BindingFlags.NonPublic | BindingFlags.Instance)
                    .SetValue(service, serviceConfiguration.AllowStop);

                // --- Allow Status Check (user can check status of given service)
                service.GetType().GetField("allowStatusCheck", BindingFlags.NonPublic | BindingFlags.Instance)
                    .SetValue(service, serviceConfiguration.AllowStatusCheck);
            }
            
            return service;
        }
    }
}
