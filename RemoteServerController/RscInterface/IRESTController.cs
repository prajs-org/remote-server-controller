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

namespace RscInterface
{
    // System namespaces
    using System.ServiceModel;
    using System.ServiceModel.Web;

    // Project namespaces
    // -- none

    [ServiceContract]
    public interface IRESTController
    {
        #region ServiceController

        /// <summary>
        /// Return current status of given service if allowed by permission configuration.
        /// See specification for possible action results.
        /// </summary>
        /// <param name="serviceName">Service name</param>
        /// <returns>Object with information about current status and result of this action.</returns>
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "JSON/Service/Status/{serviceName}?apiKey={apiKey}")]
        [OperationContract]
        ServiceStatus ServiceStatusJSON(string serviceName, string apiKey);
        /// <summary>
        /// Try to start service of given name.
        /// Success is not guaranteed.
        /// Always check action result!
        /// See specification for possible action results.
        /// </summary>
        /// <param name="serviceName">Service name</param>
        /// <returns>Object with information about result of this action.</returns>
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "JSON/Service/Start/{serviceName}?apiKey={apiKey}")]
        [OperationContract]
        ServiceActionResult ServiceStartJSON(string serviceName, string apiKey);
        /// <summary>
        /// Try to stop service of given name.
        /// Success is not guaranteed.
        /// Always check action result!
        /// See specification for possible action results.
        /// </summary>
        /// <param name="serviceName">Service name</param>
        /// <returns>Object with information about result of this action.</returns>
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "JSON/Service/Stop/{serviceName}?apiKey={apiKey}")]
        [OperationContract]
        ServiceActionResult ServiceStopJSON(string serviceName, string apiKey);

        #endregion

        #region FileController

        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "JSON/File/Read/{fileAlias}?apiKey={apiKey}")]
        [OperationContract]
        FileReadResult FileReadByAliasJSON(string fileAlias, string apiKey);

        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "JSON/File/Read/{fileAlias}/Start/{length}?apiKey={apiKey}")]
        [OperationContract]
        FileReadResult FileReadByAliasStartJSON(string fileAlias, string length, string apiKey);

        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "JSON/File/Read/{fileAlias}/End/{length}?apiKey={apiKey}")]
        [OperationContract]
        FileReadResult FileReadByAliasEndJSON(string fileAlias, string length, string apiKey);

        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "JSON/File/Read/{fileAlias}/Interval/{from}/{length}?apiKey={apiKey}")]
        [OperationContract]
        FileReadResult FileReadByAliasIntervalJSON(string fileAlias, string from, string length, string apiKey);

        #endregion
    }
}
