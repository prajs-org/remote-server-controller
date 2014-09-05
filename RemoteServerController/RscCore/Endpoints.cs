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
    using System.Text;

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
            RscLog.Debug("Going to create REST HTTP endpoint.");
            // The new host
            WebServiceHost restHost = null;
            try
            {
                // Address
                Uri uri = new Uri(String.Format("http://{0}:{1}/{2}",
                    Configurator.Settings.Network.Host,
                    Configurator.Settings.Network.Port,
                    typeof(RESTController).Name));
                RscLog.Debug("URI<{0}>", uri.ToString());
                // Try to unbind SSL from port
                UnbindSSLFromPort();
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
                RscLog.Error(ex, "Could not create REST/HTTP host");
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
            RscLog.Debug("Going to create REST HTTPS endpoint.");
            // The new host
            WebServiceHost restHost = null;
            try
            {
                // Address
                Uri uri = new Uri(String.Format("https://{0}:{1}/{2}",
                    Configurator.Settings.Network.Host,
                    Configurator.Settings.Network.Port,
                    typeof(RESTController).Name));
                RscLog.Debug("URI<{0}>", uri.ToString());
                // Bind SSL to configured port
                if (RebindSSLToPort())
                {
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
                else
                {
                    throw new InvalidOperationException("Port is already in use. Cannot free it.");
                }
            }
            catch (Exception ex)
            {
                RscLog.Error(ex, "Could not create REST/HTTPS host");
                restHost = null;
            }
            return restHost;
        }

        /// <summary>
        /// Try to bind SSL to application port.
        /// If this port is already taken, try to free it and rebind it for our purpose.
        /// NOT TESTED WITH WINDOWS XP OR WINDOWS SERVER 2003 AND OLDER!
        /// </summary>
        /// <returns>True if port was binded, false otherwise.</returns>
        private static bool RebindSSLToPort()
        {
            UnbindSSLFromPort();
            return BindSSLToPort();
        }

        /// <summary>
        /// Function bind SSL to application port using the given certificate.
        /// This allows the application to communicate via https protocol.
        /// See documentation for details about configuration of personal certificates.
        /// NOT TESTED WITH WINDOWS XP OR WINDOWS SERVER 2003 AND OLDER!
        /// </summary>
        /// <returns>True if binded correctly.</returns>
        private static bool BindSSLToPort()
        {
            string command = string.Format(@"http add sslcert ipport=0.0.0.0:{0} certhash={1} appid={{{2}}}",
                Configurator.Settings.Network.Port,
                Configurator.Settings.Network.CertificateThumbprint,
                Assembly.GetExecutingAssembly().GetType().GUID.ToString(),
                Environment.UserDomainName,
                Environment.UserName);

            if (ExecuteNetsh(command) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Function unbind SSL from application port.
        /// See documentation for details about configuration of personal certificates.
        /// NOT TESTED WITH WINDOWS XP OR WINDOWS SERVER 2003 AND OLDER!
        /// </summary>
        /// <returns>True if unbind correctly. False if could not unbind it or if the port is not bind at all.</returns>
        private static bool UnbindSSLFromPort()
        {
            string command = string.Format(@"http delete sslcert ipport=0.0.0.0:{0}",
                Configurator.Settings.Network.Port);

            if (ExecuteNetsh(command) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Execute netssh tool with given command. Use general timeout as configured.
        /// NOT TESTED WITH WINDOWS XP OR WINDOWS SERVER 2003 AND OLDER!
        /// </summary>
        private static int ExecuteNetsh(string command)
        {
            try
            {
                RscLog.Debug("Going to execute this command: <netsh {0}>", command);
                Process netsh = new Process();
                netsh.StartInfo.FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86), "netsh.exe");
                netsh.StartInfo.Arguments = command;
                netsh.StartInfo.RedirectStandardOutput = true;
                netsh.StartInfo.RedirectStandardError = true;
                netsh.StartInfo.UseShellExecute = false;
                netsh.Start();
                StringBuilder sb = new StringBuilder();
                while (!netsh.StandardOutput.EndOfStream)
                {
                    sb.Append(netsh.StandardOutput.ReadLine());
                    sb.Append(" | ");
                    // do something with line
                }
                netsh.WaitForExit(Constants.GeneralTimeout);
                RscLog.Debug("Command output: {0}", sb.ToString());
                return netsh.ExitCode;
            }
            catch (Exception ex)
            {
                RscLog.Error(ex, "Could not unbind SSL from port.");
                return -1;
            }
        }
    }
}
