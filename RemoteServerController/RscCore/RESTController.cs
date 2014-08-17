﻿/******************************************************************************
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
namespace RscCore
{
    // System namspaces
    // -- none

    // Project namespaces
    using RscInterface;
    using RscCore.Controllers;

    /// <summary>
    /// Implementation of REST controller.
    /// Each function supported by this application and reachable via REST technology can be executed within this controller.
    /// Used by WCF by default but can be used directly wherever within application.
    /// See IRESTController interface for description of each function.
    /// </summary>
    public class RESTController : IRESTController
    {
        public ServiceStatus ServiceStatusJSON(string serviceName)
        {
            return ControlFactory.GetService(serviceName).GetStatus();
        }

        public ServiceActionResult ServiceStartJSON(string serviceName)
        {
            return ControlFactory.GetService(serviceName).Start();
        }

        public ServiceActionResult ServiceStopJSON(string serviceName)
        {
            return ControlFactory.GetService(serviceName).Stop();
        }
    }
}
