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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using RscConfig;
using RscLog;

namespace RscCore.Controllers
{
    public static class ControlFactory
    {
        /// <summary>
        /// Create instance of service with configured permissions.
        /// </summary>
        /// <param name="serviceName">Name of service</param>
        /// <returns>Instance of Service</returns>
        public static Service GetService(string serviceName)
        {
            // Load configuration
            AddService config;
            bool serviceConfigured = Configurator.Settings.Services.AllowedServices.GetService(serviceName, out config);
            // Set permissions (or no permissions if service is not configurated)
            Service service = new Service(serviceName);
            service.GetType().GetField("allowStart", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(service, config == null ? false : config.AllowStart);
            service.GetType().GetField("allowStop", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(service, config == null ? false : config.AllowStop);
            service.GetType().GetField("allowStatusCheck", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(service, config == null ? false : config.AllowStatusCheck);
            return service;
        }
    }
}
