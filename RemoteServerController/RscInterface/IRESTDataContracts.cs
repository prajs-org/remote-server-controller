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
namespace RscInterface
{
    // System namespaces
    using System.Runtime.Serialization;
    using System.ServiceProcess;

    // Project namespaces
    using RscConfig;

    /// <summary>
    /// Data contract for checking of service status.
    /// </summary>
    [DataContract]
    public class ServiceStatus
    {
        /// <summary>
        /// Service current status
        /// </summary>
        [DataMember]
        public string Status { get; set; }
        /// <summary>
        /// Service name
        /// </summary>
        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// Result of checking service status
        /// </summary>
        [DataMember]
        public string Result { get; set; }
        /// <summary>
        /// Data contract with result of checking of service status.
        /// </summary>
        /// <param name="name">Service name</param>
        /// <param name="status">Current service status</param>
        /// <param name="result">Result of checking of service status</param>
        public ServiceStatus(string name, ServiceControllerStatus? status, ReturnCodes.ActionReturnCode result)
        {
            this.Name = name;
            this.Status = status.HasValue ? status.ToString() : Constants.UknownToken;
            this.Result = result.ToString();
        }
    }
    /// <summary>
    /// Data contract for changing service status.
    /// </summary>
    [DataContract]
    public class ServiceActionResult
    {
        /// <summary>
        /// Service new status (after change) - does not need to be desired status, always check Result!
        /// </summary>
        [DataMember]
        public string Status { get; set; }
        /// <summary>
        /// Service name
        /// </summary>
        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// Result of changing service status
        /// </summary>
        [DataMember]
        public string Result { get; set; }
        /// <summary>
        /// Data contract with result of changing of service status.
        /// </summary>
        /// <param name="name">Service name</param>
        /// <param name="status">Service status after change</param>
        /// <param name="result">Result of changing of service status</param>
        public ServiceActionResult(string name, ServiceControllerStatus? status, ReturnCodes.ActionReturnCode result)
        {
            this.Name = name;
            this.Status = status.HasValue ? status.ToString() : Constants.UknownToken;
            this.Result = result.ToString();
        }
    }

    /// <summary>
    /// Data contract for reading a file.
    /// </summary>
    [DataContract]
    public class FileReadResult
    {
        /// <summary>
        /// Full path to file
        /// </summary>
        [DataMember]
        public string FullPath { get; set; }
        /// <summary>
        /// Result of reading
        /// </summary>
        [DataMember]
        public string Result { get; set; }
        /// <summary>
        /// Content of file
        /// </summary>
        [DataMember]
        public string Content {get;set;}
        /// <summary>
        /// Data contract with result of action on file.
        /// </summary>
        /// <param name="fullPath">Service name</param>
        /// <param name="content">Content of file</param>
        /// <param name="result">Result of reading</param>
        public FileReadResult(string fullPath, ReturnCodes.ActionReturnCode result, string content)
        {
            this.FullPath = fullPath;
            this.Result = result.ToString();
            this.Content = content;
        }
    }
}
