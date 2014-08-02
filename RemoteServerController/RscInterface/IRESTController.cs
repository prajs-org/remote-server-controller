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
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace RscInterface
{
    [ServiceContract]
    public interface IRESTController
    {
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "JSON/Service/Status/{serviceName}")]
        [OperationContract]
        ServiceStatus ServiceStatusJSON(string serviceName);

        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "JSON/Service/Start/{serviceName}")]
        [OperationContract]
        ServiceActionResult ServiceStartJSON(string serviceName);

        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "JSON/Service/Stop/{serviceName}")]
        [OperationContract]
        ServiceActionResult ServiceStopJSON(string serviceName);
    }
}