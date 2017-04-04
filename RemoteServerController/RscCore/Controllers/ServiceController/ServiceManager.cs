/******************************************************************************
 * Remote Server Controller                                                   *
 * https://github.com/prajs-org/remote-server-controller                      *
 * Copyright (C) 2014-217 Karel Prajs, karel@prajs.org                        *
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

namespace RscCore.Controllers.ServiceController
{
    // System namespaces
    using System;
    using System.Linq;
    using System.ServiceProcess;

    // Project namespaces
    using RscLog;
    using RscConfig;
    using RscInterface;

    /// <summary>
    /// Controller class designated for controlling of Windows Managed Services.
    /// Each object of class ServiceManager can handle single Windows Managed ServiceManager.
    /// </summary>
    public sealed class ServiceManager
    {
        #region Construction

        /// <summary>
        /// Create instance of ServiceManager class. Instance can be created only by ControlFactory. If you create it directly, all permissions will be set to false.
        /// </summary>
        /// <param name="serviceName">Name of service.</param>
        public ServiceManager(string serviceName)
        {
            this.Name = serviceName;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Service name
        /// </summary>
        public string Name
        {
            internal set;
            get;
        }

        #endregion

        #region Security properties

        /// <summary>
        /// Flag if service is allowed to be started. Can be set by ControlFactory only.
        /// </summary>
        private bool allowStart = false;
        /// <summary>
        /// Flag if service can be started.
        /// </summary>
        public bool AllowStart
        {
            get { return this.allowStart; }
        }

        /// <summary>
        /// Flag if service is allowed to be stopped. Can be set by ControlFactory only.
        /// </summary>
        private bool allowStop = false;
        /// <summary>
        /// Flag if service can be started.
        /// </summary>
        public bool AllowStop
        {
            get { return this.allowStop; }
        }

        /// <summary>
        /// Flag if service is allowed to be started. Can be set by ControlFactory only.
        /// </summary>
        private bool allowStatusCheck = false;
        /// <summary>
        /// Flag if service can be started.
        /// </summary>
        public bool AllowStatusCheck
        {
            get { return this.allowStatusCheck; }
        }

        #endregion

        #region Public functions

        /// <summary>
        /// Start service. Has no effect if current status differs from Stopped or Paused.
        /// </summary>
        /// <returns>Action result</returns>
        public ServiceActionResult Start()
        {
            if (this.AllowStart)
            {
                // If action is allowed, StatusCheck has to be considered as allowed as well.
                return this.ChangeServiceStatus(ServiceControllerStatus.Running, ServiceControllerStatus.Stopped, ServiceControllerStatus.Paused);
            }
            else
            {
                RscLog.AuditFailed("Service<{0}> is not allowed to be started!", this.Name);
                if (this.AllowStatusCheck)
                {
                    return new ServiceActionResult(this.Name, this.GetStatusToken(), ReturnCodes.ActionReturnCode.NotAllowed);
                }
                else
                {
                    return new ServiceActionResult(this.Name, null, ReturnCodes.ActionReturnCode.NotAllowed);
                }
            }
        }
        /// <summary>
        /// Stop service. Has no effect if current status differs from Running.
        /// </summary>
        /// <returns>Action result</returns>
        public ServiceActionResult Stop()
        {
            if (this.AllowStop)
            {
                // If action is allowed, StatusCheck has to be considered as allowed as well.
                return this.ChangeServiceStatus(ServiceControllerStatus.Stopped, ServiceControllerStatus.Running);
            }
            else
            {
                RscLog.AuditFailed("Service<{0}> is not allowed to be stopped!", this.Name);
                if (this.AllowStatusCheck)
                {
                    return new ServiceActionResult(this.Name, this.GetStatusToken(), ReturnCodes.ActionReturnCode.NotAllowed);
                }
                else
                {
                    return new ServiceActionResult(this.Name, null, ReturnCodes.ActionReturnCode.NotAllowed);
                }
            }            
        }
        /// <summary>
        /// Get status of service.
        /// </summary>
        /// <returns></returns>
        public ServiceStatus GetStatus()
        {
            if (this.AllowStatusCheck)
            {
                var status = GetStatusToken();
                return new ServiceStatus(this.Name, status, status == null ? ReturnCodes.ActionReturnCode.UnknownError : ReturnCodes.ActionReturnCode.OK);
            }
            else
            {
                RscLog.AuditFailed("Check of status of service<{0}> is not allowed!", this.Name);
                return new ServiceStatus(this.Name, null, ReturnCodes.ActionReturnCode.NotAllowed);
            }
        }
        /// <summary>
        /// Return status of service. Return null in case of error.
        /// </summary>
        /// <param name="serviceName">Name of service</param>
        /// <returns>Current service status</returns>
        public ServiceControllerStatus? GetStatusToken()
        {
            ServiceControllerStatus? status;

            using (ServiceController sc = new ServiceController(this.Name))
            {
                try
                {
                    status = sc.Status;
                }
                catch
                {
                    status = null as ServiceControllerStatus?;
                }
            }

            return status;
        }

        #endregion

        #region Private functions

        /// <summary>
        /// Function will try to change status of service according to argument newStatus.
        /// The change is only allowed if current status of service is within the list given by argument allowedCurrentStatuses.
        /// </summary>
        /// <param name="serviceName">Name of service</param>
        /// <param name="newStatus">New status</param>
        /// <param name="allowedCurrentStatuses">Service has to have one of these statuses, otherwise the change fails.</param>
        /// <returns>Allways returns ActionResult</returns>
        private ServiceActionResult ChangeServiceStatus(ServiceControllerStatus newStatus, params ServiceControllerStatus[] allowedCurrentStatuses)
        {
            var currentStatus = GetStatusToken();
            ReturnCodes.ActionReturnCode error_code = ReturnCodes.ActionReturnCode.OK;

            if (currentStatus.HasValue)
            {
                // Check if current status is in allowed statuses
                if (!allowedCurrentStatuses.Contains(currentStatus.Value))
                {
                    error_code = ReturnCodes.ActionReturnCode.UnmetRequirements;
                    RscLog.Alert("Status of service<{0}> cannot be changed to<{1}> because current currentStatus<{2}> is not in allowed statuses.",
                        this.Name,
                        newStatus,
                        currentStatus);
                }
                else
                {
                    using (ServiceController service = new ServiceController(this.Name))
                    {
                        try
                        {
                            TimeSpan timeout = TimeSpan.FromMilliseconds(Configurator.Settings.Services.StatusChangeTimeout);
                            switch (newStatus)
                            {
                                case ServiceControllerStatus.Running:
                                    service.Start();
                                    break;
                                case ServiceControllerStatus.Stopped:
                                    service.Stop();
                                    break;
                                case ServiceControllerStatus.Paused:
                                    service.Pause();
                                    break;
                                default:
                                    RscLog.Error("Status of service<{0}> cannot be changed because new status<{1}> is not supported.",
                                        this.Name,
                                        newStatus);
                                    error_code = ReturnCodes.ActionReturnCode.NotSupported;
                                    break;
                            }
                            service.WaitForStatus(newStatus, timeout);
                            currentStatus = GetStatusToken();
                        }
                        catch (System.ServiceProcess.TimeoutException)
                        {
                            error_code = ReturnCodes.ActionReturnCode.Timeout;
                            RscLog.Alert("Change of status of service<{0}> failed on timeout.", this.Name);
                        }
                        catch (Exception ex)
                        {
                            error_code = ReturnCodes.ActionReturnCode.UnknownError;
                            RscLog.Error(ex, "Change of status failed.");
                        }
                    }
                }
            }
            else
            {
                error_code = ReturnCodes.ActionReturnCode.UnmetRequirements;
                RscLog.Error("Status of service<{0}> cannot be changed because current status cannot be determined.", this.Name);
            }
            return new ServiceActionResult(this.Name, currentStatus, error_code);
        }

        #endregion
    }
}
