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
    public class Service
    {

// --- Constructors ----------------------------------------------------

        /// <summary>
        /// Create instance of Service class. Instance can be created only by ControlFactory. If you create it directly, all permissions will be set to false.
        /// </summary>
        /// <param name="serviceName">Name of service.</param>
        public Service(string serviceName)
        {
            this.Name = serviceName;
        }

// --- Public properties ----------------------------------------------------

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

// --- Security properties ----------------------------------------------------

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

// --- Public functions ----------------------------------------------------

        #region Public functions

        /// <summary>
        /// Start service. Has no effect if current status differs from Stopped or Paused.
        /// </summary>
        /// <returns>Action result</returns>
        public ServiceActionResult Start()
        {
            Log.Info("Incoming request: Service/Start/{0}", this.Name);
            if (this.AllowStart)
            {
                // If action is allowed, StatusCheck has to be considered as allowed as well.
                return this.ChangeServiceStatus(ServiceControllerStatus.Running, ServiceControllerStatus.Stopped, ServiceControllerStatus.Paused);
            }
            else
            {
                Log.Warning("Request Service/Start/{0} not allowed", this.Name);
                if (this.AllowStatusCheck)
                {
                    return new ServiceActionResult(this.Name, this.GetStatusToken(), Constants.ErrorCode.NotAllowed);
                }
                else
                {
                    return new ServiceActionResult(this.Name, null, Constants.ErrorCode.NotAllowed);
                }
            }
        }
        /// <summary>
        /// Stop service. Has no effect if current status differs from Running.
        /// </summary>
        /// <returns>Action result</returns>
        public ServiceActionResult Stop()
        {
            Log.Info("Incoming request: Service/Stop/{0}", this.Name);
            if (this.AllowStop)
            {
                // If action is allowed, StatusCheck has to be considered as allowed as well.
                return this.ChangeServiceStatus(ServiceControllerStatus.Stopped, ServiceControllerStatus.Running);
            }
            else
            {
                Log.Warning("Request Service/Stop/{0} not allowed", this.Name);
                if (this.AllowStatusCheck)
                {
                    return new ServiceActionResult(this.Name, this.GetStatusToken(), Constants.ErrorCode.NotAllowed);
                }
                else
                {
                    return new ServiceActionResult(this.Name, null, Constants.ErrorCode.NotAllowed);
                }
            }            
        }
        /// <summary>
        /// Get status of service.
        /// </summary>
        /// <returns></returns>
        public ServiceStatus GetStatus()
        {
            Log.Info("Incoming request: Service/Status/{0}", this.Name);
            if (this.AllowStatusCheck)
            {
                var status = GetStatusToken();
                return new ServiceStatus(this.Name, status, status == null ? Constants.ErrorCode.UnknownError : Constants.ErrorCode.OK);
            }
            else
            {
                Log.Warning("Request Service/Status/{0} not allowed", this.Name);
                return new ServiceStatus(this.Name, null, Constants.ErrorCode.NotAllowed);
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

// --- Private functions ----------------------------------------------------

        #region Private functions

        /// <summary>
        /// Change status of service.
        /// </summary>
        /// <param name="serviceName">Name of service</param>
        /// <param name="newStatus">New status</param>
        /// <param name="allowedCurrentStatuses">Service has to have one of these statuses, otherwise the change fails.</param>
        /// <returns>ActionResult</returns>
        private ServiceActionResult ChangeServiceStatus(ServiceControllerStatus newStatus, params ServiceControllerStatus[] allowedCurrentStatuses)
        {
            var status = GetStatusToken();
            Constants.ErrorCode error_code = Constants.ErrorCode.OK;

            if (status.HasValue)
            {
                if (!allowedCurrentStatuses.Contains(status.Value))
                {
                    error_code = Constants.ErrorCode.UnmetRequirements;
                    Log.Info("ServiceStart<{0}> not in allowed status.", this.Name);
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
                                    Log.Error("Unsupported action<{0}>.", newStatus);
                                    error_code = Constants.ErrorCode.NotSupported;
                                    break;
                            }
                            service.WaitForStatus(newStatus, timeout);
                            status = GetStatusToken();
                        }
                        catch (System.ServiceProcess.TimeoutException)
                        {
                            error_code = Constants.ErrorCode.Timeout;
                            Log.Warning("Service<{0}> status change failed. TimeoutException.", this.Name);
                        }
                        catch (Exception ex)
                        {
                            error_code = Constants.ErrorCode.UnknownError;
                            Log.Error("Service<{0}> status change failed. Exception: {1}", this.Name, ex.InnerException == null ? ex.Message : ex.InnerException.Message);
                        }
                    }
                }
            }
            else
            {
                error_code = Constants.ErrorCode.UnmetRequirements;
                Log.Error("Service<{0}> has unknown status. Cannot change status.", this.Name);
            }
            return new ServiceActionResult(this.Name, status, error_code);
        }

        #endregion
    }
}
