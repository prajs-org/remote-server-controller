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
using System.Runtime.Serialization;
using System.ServiceProcess;

using RscConfig;

namespace RscInterface
{
    [DataContract]
    public class ServiceStatus
    {
        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string Name { get; set; }

        public ServiceStatus(string name, ServiceControllerStatus? status)
        {
            this.Name = name;
            this.Status = status.HasValue ? status.ToString() : Configurator.UnknownStatusToken;
        }
    }

    [DataContract]
    public class ServiceActionResult
    {
        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Success { get; set; }

        public ServiceActionResult(string name, ServiceControllerStatus? status, bool success)
        {
            this.Name = name;
            this.Status = status.HasValue ? status.ToString() : Configurator.UnknownStatusToken;
            this.Success = success ? Configurator.True : Configurator.False;
        }
    }
}
