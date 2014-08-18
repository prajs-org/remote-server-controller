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
    using System.Reflection;

    // Projet namespaces
    using RscConfig;
    using RscLog;

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
        public static Service GetService(string serviceName)
        {
            Service service = new Service(serviceName);
            AddService serviceConfiguration;
            // Forbid everything unless configuration is found.
            bool forbidAll = true;
            // Load configuration of service from configuration file
            if (Configurator.Settings.Services.AllowedServices.GetService(serviceName, out serviceConfiguration))
            {
                // Okay, service is configured - remove the forbidAll flag.
                forbidAll = false;
            }
            else
            {
                Log.Warning("Processing of service<{0}> is not allowed!", serviceName);
            }
            // --- Allow Start (service may be started)
            service.GetType().GetField("allowStart", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(service, forbidAll ? false : serviceConfiguration.AllowStart);

            // --- Allow Stop (service may be stopped)
            service.GetType().GetField("allowStop", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(service, forbidAll ? false : serviceConfiguration.AllowStop);

            // --- Allow Status Check (user can check status of given service)
            service.GetType().GetField("allowStatusCheck", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(service, forbidAll ? false : serviceConfiguration.AllowStatusCheck);
            
            return service;
        }
    }
}
