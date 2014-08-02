﻿/*
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
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;

using RscLog;
using RscInterface;

namespace RscCore
{
    public class RESTController : IRESTController
    {
        public ServiceStatus ServiceStatusJSON(string serviceName)
        {
            return Controllers.Service.GetServiceStatus(serviceName);
        }

        public ServiceActionResult ServiceStartJSON(string serviceName)
        {
            return Controllers.Service.ServiceStart(serviceName);
        }

        public ServiceActionResult ServiceStopJSON(string serviceName)
        {
            return Controllers.Service.ServiceStop(serviceName);
        }
    }
}