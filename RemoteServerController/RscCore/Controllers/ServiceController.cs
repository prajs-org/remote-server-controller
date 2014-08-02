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
using System.ServiceProcess;

using RscLog;
using RscConfig;
using RscInterface;

namespace RscCore.Controllers
{
    class Service
    {
        public static ServiceActionResult ServiceStart(string serviceName)
        {
            Log.Debug("Incoming request: Service/Start/{0}", serviceName);
            return ServiceHelper.ChangeServiceStatus(serviceName, ServiceControllerStatus.Running, ServiceControllerStatus.Stopped);
        }

        public static ServiceActionResult ServiceStop(string serviceName)
        {
            Log.Debug("Incoming request: Service/Stop/{0}", serviceName);
            return ServiceHelper.ChangeServiceStatus(serviceName, ServiceControllerStatus.Stopped, ServiceControllerStatus.Running);
        }

        public static ServiceStatus ServiceStatus(string serviceName)
        {
            Log.Debug("Incoming request: Service/Status/{0}", serviceName);
            return new ServiceStatus(serviceName, ServiceHelper.GetServiceStatusToken(serviceName));
        }
    }
}
