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

namespace RscCore
{
    // System namspaces
    // -- none

    // Project namespaces
    using RscInterface;
    using RscCore.Controllers;
    using RscLog;
    using System;
    using RscCore.Factories;
    using System.Collections.Generic;

    /// <summary>
    /// Implementation of REST controller.
    /// Each function supported by this application and reachable via REST technology can be executed within this controller.
    /// Used by WCF by default but can be used directly wherever within application.
    /// See IRESTController interface for description of each function.
    /// </summary>
    public class RESTController : IRESTController
    {
        const string serviceLogMessage =    "Incoming request<{0}> for service<{1}>.";
        const string fileLogMessage =       "Incoming request<{0}> for file<{1}>.";
        const string exInvalidInteger =     "Value<{0}> is not valid Integer.";
        const string exInvalidInteger2 =    "Values <{0}> and/or <{1}> are not valid Integer.";

        #region ServiceController

        public ServiceStatus ServiceStatusJSON(string serviceName, string apiKey)
        {
            RscLog.AuditIncoming(apiKey, serviceLogMessage, "SERVICE STATUS", serviceName);
            return ServiceManagerFactory.CreateServiceManager(serviceName, apiKey).GetStatus();
        }

        public ServiceActionResult ServiceStartJSON(string serviceName, string apiKey)
        {
            RscLog.AuditIncoming(apiKey, serviceLogMessage, "START SERVICE", serviceName);
            return ServiceManagerFactory.CreateServiceManager(serviceName, apiKey).Start();
        }

        public ServiceActionResult ServiceStopJSON(string serviceName, string apiKey)
        {
            RscLog.AuditIncoming(apiKey, serviceLogMessage, "STOP SERVICE", serviceName);
            return ServiceManagerFactory.CreateServiceManager(serviceName, apiKey).Stop();
        }

        public List<ServiceStatus> AllServicesStatusJSON(string apiKey)
        {
            List<ServiceStatus> serviceStatuses = new List<ServiceStatus>();
            RscLog.AuditIncoming(apiKey, serviceLogMessage, "SERVICE STATUS", RscConfig.Constants.AllItems);
            foreach(var item in RscConfig.Configurator.Settings.Services.AllowedServices)
            {
                if(item is RscConfig.AddService)
                {
                    var allowedService = (RscConfig.AddService)item;
                    var servisStatus = ServiceManagerFactory.CreateServiceManager(allowedService.Name, apiKey).GetStatus();
                    serviceStatuses.Add(servisStatus);
                }
            }
            return serviceStatuses;
        }

        #endregion
    }
}
