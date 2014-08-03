using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RscConfig;

namespace RscCore.Controllers
{
    public static class ServiceSecurity
    {
        /// <summary>
        /// Return true if service is allowed for actions.
        /// </summary>
        /// <param name="serviceName">Name of service</param>
        /// <returns>True if service is allowed for action</returns>
        public static bool IsActionAllowed(string serviceName, Constants.ServiceActions action)
        {
            foreach (var item in Configurator.Settings.Services.AllowedServices)
            {
                AddService service = (AddService)item;
                if (serviceName == service.Name)
                {
                    switch (action)
                    {
                        case Constants.ServiceActions.Start:
                            return service.AllowStart;
                        case Constants.ServiceActions.Stop:
                            return service.AllowStop;
                        case Constants.ServiceActions.StatusCheck:
                            return service.AllowStatusCheck;
                        default:
                            return false;
                    }
                }
            }
            return false;
        }
    }
}
