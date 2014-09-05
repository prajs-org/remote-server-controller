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

        #endregion

        #region FileController

        public FileReadResult FileReadByAliasJSON(string fileAlias, string apiKey)
        {
            RscLog.AuditIncoming(apiKey, fileLogMessage, "READ FILE", fileAlias);
            return FileManagerFactory.CreateFileManager(fileAlias, apiKey).Read();
        }

        public FileReadResult FileReadByAliasStartJSON(string fileAlias, string length, string apiKey)
        {
            RscLog.AuditIncoming(apiKey, fileLogMessage, "READ FILE START", fileAlias);
            try
            {
                int iLength = Convert.ToInt32(length);
                return FileManagerFactory.CreateFileManager(fileAlias, apiKey).ReadStart(iLength);
            }
            catch
            {
                RscLog.Error(exInvalidInteger, length);
                return new FileReadResult(fileAlias, ReturnCodes.ActionReturnCode.FormatError, String.Empty);
            }
        }

        public FileReadResult FileReadByAliasEndJSON(string fileAlias, string length, string apiKey)
        {
            RscLog.AuditIncoming(apiKey, fileLogMessage, "READ FILE END", fileAlias);
            try
            {
                int iLength = Convert.ToInt32(length);
                return FileManagerFactory.CreateFileManager(fileAlias, apiKey).ReadEnd(iLength);
            }
            catch
            {
                RscLog.Error(exInvalidInteger, length);
                return new FileReadResult(fileAlias, ReturnCodes.ActionReturnCode.FormatError, String.Empty);
            }
        }

        public FileReadResult FileReadByAliasIntervalJSON(string fileAlias, string from, string length, string apiKey)
        {
            RscLog.AuditIncoming(apiKey, fileLogMessage, "READ FILE INTERVAL", fileAlias);
            try
            {
                int iFrom = Convert.ToInt32(from);
                int iLength = Convert.ToInt32(length);
                return FileManagerFactory.CreateFileManager(fileAlias, apiKey).ReadInterval(iFrom, iLength);
            }
            catch
            {
                RscLog.Error(exInvalidInteger2, from, length);
                return new FileReadResult(fileAlias, ReturnCodes.ActionReturnCode.FormatError, String.Empty);
            }
        }

        #endregion
    }
}
