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
    // System namespaces
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using System.ServiceModel.Description;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;

    // Project namespaces
    using RscConfig;
    using RscLog;

    /// <summary>
    /// Static class provides static function construction various WCF endpoints.
    /// </summary>
    public static class Endpoints
    {
        /// <summary>
        /// Create and return new REST endpoint with no security.
        /// Common HTTP protocol is used.
        /// No credentials are required to use this endpoint.
        /// Configuration is taken directly from configuration file.
        /// Cross Domain Scripts are always enabled.
        /// </summary>
        /// <returns>New HTTP endpoint</returns>
        public static WebServiceHost GetRESTHost()
        {
            // The new host
            WebServiceHost restHost = null;
            try
            {
                // Address
                Uri uri = new Uri(String.Format("http://{0}:{1}/{2}",
                    Configurator.Settings.Network.Host,
                    Configurator.Settings.Network.Port,
                    typeof(RESTController).Name));
                // Create host
                restHost = new WebServiceHost(typeof(RESTController));
                // No security
                WebHttpBinding binding = new WebHttpBinding(WebHttpSecurityMode.None);
                // Allow cross domain scripts
                binding.CrossDomainScriptAccessEnabled = Configurator.Settings.Network.CrossDomainScriptAccessEnabled;
                // Create endpoint
                ServiceEndpoint endPoint = new ServiceEndpoint(ContractDescription.GetContract(typeof(RESTController)),
                                                               binding,
                                                               new EndpointAddress(uri));
                // Return the new host
                restHost.AddServiceEndpoint(endPoint);
                restHost.Authorization.ServiceAuthorizationManager = new AuthorizationManager();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Could not create REST/HTTP host");
                restHost = null;
            }
            return restHost;
        }

        /// <summary>
        /// Create and return new REST endpoint with no security.
        /// Secured HTTPS protocol is used.
        /// No credentials are required to use this endpoint.
        /// Configuration is taken directly from configuration file.
        /// Cross Domain Scripts are always enabled.
        /// This function required special configuration and valid personal certificate, see documentation for more details.
        /// </summary>
        /// <returns>New HTTPS endpoint</returns>
        public static WebServiceHost GetRESTHostSSL()
        {
            // The new host
            WebServiceHost restHost = null;
            try
            {
                // Address
                Uri uri = new Uri(String.Format("https://{0}:{1}/{2}",
                    Configurator.Settings.Network.Host,
                    Configurator.Settings.Network.Port,
                    typeof(RESTController).Name));
                // Bind SSL to configured port
                BindSSLToPort();
                // Create host
                restHost = new WebServiceHost(typeof(RESTController));
                // Use SSL
                WebHttpBinding binding = new WebHttpBinding(WebHttpSecurityMode.Transport);
                // Allow cross domain scripts
                binding.CrossDomainScriptAccessEnabled = Configurator.Settings.Network.CrossDomainScriptAccessEnabled;
                // Create endpoint
                ServiceEndpoint endPoint = new ServiceEndpoint(ContractDescription.GetContract(typeof(RESTController)),
                                                               binding,
                                                               new EndpointAddress(uri));
                // Return the new host
                restHost.AddServiceEndpoint(endPoint);
                restHost.Authorization.ServiceAuthorizationManager = new AuthorizationManager();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Could not create REST/HTTPS host");
                restHost = null;
            }
            return restHost;
        }

        /// <summary>
        /// Function bind SSL to application port using the given certificate.
        /// This allows the application to communicate via https protocol.
        /// See documentation for details about configuration of personal certificates.
        /// NOT TESTED WITH WINDOWS XP OR WINDOWS SERVER 2003 AND OLDER!
        /// </summary>
        private static void BindSSLToPort()
        {
            try
            {
                Process bindSSLToPort = new Process();
                bindSSLToPort.StartInfo.FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86), "netsh.exe");
                bindSSLToPort.StartInfo.Arguments = string.Format(@"netsh http add sslcert ipport=0.0.0.0:{0} certhash={1} appid={{{2}}}",
                    Configurator.Settings.Network.Port,
                    Configurator.Settings.Network.CertificateThumbprint,
                    Assembly.GetExecutingAssembly().GetType().GUID.ToString(),
                    Environment.UserDomainName,
                    Environment.UserName);
                bindSSLToPort.Start();
                bindSSLToPort.WaitForExit(Constants.GeneralTimeout);
                // ExitCode other then zero does not need to be a problem. But warn about it.
                if (bindSSLToPort.ExitCode > 0)
                {
                    Log.Warning("Binding SSL to port<{0}> exited with ExitCode<{1}>.", Configurator.Settings.Network.Port, bindSSLToPort.ExitCode);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Could not bind SSL to port");
                throw;
            }
        }
    }
}
