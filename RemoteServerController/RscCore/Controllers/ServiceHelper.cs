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
using RscInterface;
using RscLog;
using RscConfig;

namespace RscCore.Controllers
{
    class ServiceHelper
    {
        /// <summary>
        /// Return status of service. Return null in case of error.
        /// </summary>
        /// <param name="serviceName">Name of service</param>
        /// <returns>Current service status</returns>
        public static ServiceControllerStatus? GetServiceStatusToken(string serviceName)
        {
            ServiceControllerStatus? status;

            using (ServiceController sc = new ServiceController(serviceName))
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
        /// <summary>
        /// Check if status is pending status. That means service is changing its status right now.
        /// </summary>
        /// <param name="status">Status you want to check</param>
        /// <returns>True if status is pending status</returns>
        public static bool IsPendingStatus(ServiceControllerStatus status)
        {
            if (status == ServiceControllerStatus.ContinuePending
                || status == ServiceControllerStatus.PausePending
                || status == ServiceControllerStatus.StartPending
                || status == ServiceControllerStatus.StopPending)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Change status of service.
        /// </summary>
        /// <param name="serviceName">Name of service</param>
        /// <param name="newStatus">New status</param>
        /// <param name="allowedCurrentStatuses">Service has to have one of these statuses, otherwise the change fails.</param>
        /// <returns></returns>
        public static ServiceActionResult ChangeServiceStatus(string serviceName, ServiceControllerStatus newStatus, params ServiceControllerStatus[] allowedCurrentStatuses)
        {
            Log.Debug("Processing request: ChangeServiceStatus<{0}> to status<{1}>", serviceName, newStatus);

            var status = ServiceHelper.GetServiceStatusToken(serviceName);
            Constants.ErrorCode error_code = Constants.ErrorCode.OK;

            if (status.HasValue)
            {
                if (!allowedCurrentStatuses.Contains(status.Value))
                {
                    error_code = Constants.ErrorCode.UnmetRequirements;
                    Log.Error("ServiceStart<{0}> not in allowed status.", serviceName);
                }
                else
                {
                    using (ServiceController service = new ServiceController(serviceName))
                    {
                        try
                        {
                            TimeSpan timeout = TimeSpan.FromMilliseconds(Configurator.Settings.Services.StatusChangeTimeout);// ?? Configurator.Settings.GeneralSettings.DefaultTimeout);
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
                            status = ServiceHelper.GetServiceStatusToken(serviceName);
                        }
                        catch (System.ServiceProcess.TimeoutException)
                        {
                            error_code = Constants.ErrorCode.Timeout;
                            Log.Error("Service<{0}> status change failed. TimeoutException.", serviceName);
                        }
                        catch (Exception ex)
                        {
                            error_code = Constants.ErrorCode.UnknownError;
                            Log.Error("Service<{0}> status change failed. Exception: {1}", serviceName, ex.InnerException == null ? ex.Message : ex.InnerException.Message);
                        }
                    }
                }
            }
            else
            {
                error_code = Constants.ErrorCode.UnmetRequirements;
                Log.Error("Service<{0}> has unknown status. Cannot change status.", serviceName);
            }
            return new ServiceActionResult(serviceName, status, error_code);
        }
    }
}
