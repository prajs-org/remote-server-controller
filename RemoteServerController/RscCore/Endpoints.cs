using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Web;
using System.ServiceModel;
using RscConfig;
using System.ServiceModel.Description;
using RscLog;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using RscInterface;

namespace RscCore
{
    public static class Endpoints
    {
        /// <summary>
        /// Create and return new REST endpoint.
        /// </summary>
        /// <returns></returns>
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
                Log.Error("Could not create REST host: "
                    + ex.Message
                    + ", "
                    + ex.InnerException == null ? String.Empty : ex.InnerException.Message);
                restHost = null;
            }
            return restHost;
        }
        /// <summary>
        /// Create and return new REST endpoint secured by SSL.
        /// </summary>
        /// <returns></returns>
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
                // No security
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
                Log.Error(ex, "Could not create REST host");
                restHost = null;
            }
            return restHost;
        }
        /// <summary>
        /// Bind SSL to application port using the given certificate. This allows the application to communicate via https protocol.
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
