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

namespace RscConfig
{
    // System namespaces
    using System;
    using System.Configuration;
    using System.Text.RegularExpressions;

    // Project namespaces
    // -- none

    #region Base
    public class Configurator : ConfigurationSection
    {
        private static Configurator instance;

        static Configurator()
        {
            instance = ConfigurationManager.GetSection("Configurator") as Configurator;
        }

        public static Configurator Settings
        {
            get{ return instance; }
        }

        [ConfigurationProperty("GeneralSettings", IsRequired = true)]
        public GeneralSettings GeneralSettings
        {
            set { this["GeneralSettings"] = value; }
            get { return (GeneralSettings)this["GeneralSettings"]; }
        }

        [ConfigurationProperty("Network", IsRequired = true)]
        public Network Network
        {
            set { this["Network"] = value; }
            get { return (Network)this["Network"]; }
        }

        [ConfigurationProperty("Services", IsRequired = true)]
        public Services Services
        {
            set { this["Services"] = value; }
            get { return (Services)this["Services"]; }
        }

        [ConfigurationProperty("Files", IsRequired = true)]
        public Files Files
        {
            set { this["Files"] = value; }
            get { return (Files)this["Files"]; }
        }

        [ConfigurationProperty("Security", IsRequired = true)]
        public Security Security
        {
            set { this["Security"] = value; }
            get { return (Security)this["Security"]; }
        }
    }
    #endregion

    #region General settings

    public class GeneralSettings : ConfigurationElement
    {
        [ConfigurationProperty("QuitToken", DefaultValue = "quit!", IsRequired = true)]
        [StringValidator(MinLength = 1)]
        public string QuitToken
        {
            set { this["QuitToken"] = value; }
            get { return (string)this["QuitToken"]; }
        }
    }

    #endregion

    #region Network

    public class Network : ConfigurationElement
    {
        [ConfigurationProperty("Host", DefaultValue = "localhost", IsRequired = true)]
        public string Host
        {
            set { this["Host"] = value; }
            get { return (string)this["Host"]; }
        }

        [ConfigurationProperty("Port", DefaultValue = "55011", IsRequired = true)]
        [LongValidator(MinValue = 0, MaxValue = long.MaxValue)]
        public long Port
        {
            set { this["Port"] = value; }
            get { return (long)this["Port"]; }
        }

        [ConfigurationProperty("UseSSL", DefaultValue = false, IsRequired = true)]
        public bool UseSSL
        {
            set { this["UseSSL"] = value; }
            get { return (bool)this["UseSSL"]; }
        }

        [ConfigurationProperty("CertificateThumbprint", DefaultValue = null, IsRequired = true)]
        public string CertificateThumbprint
        {
            set { this["CertificateThumbprint"] = value; }
            get { return Regex.Replace((string)this["CertificateThumbprint"], @"\s+", String.Empty); }
        }

        [ConfigurationProperty("CrossDomainScriptAccessEnabled", DefaultValue = false, IsRequired = true)]
        public bool CrossDomainScriptAccessEnabled
        {
            set { this["CrossDomainScriptAccessEnabled"] = value; }
            get { return (bool)this["CrossDomainScriptAccessEnabled"]; }
        }
    }

    #endregion

    #region Services

    public class Services : ConfigurationElement
    {
        [ConfigurationProperty("StatusChangeTimeout", DefaultValue = "5000", IsRequired = true)]
        [LongValidator(MinValue = 0, MaxValue = long.MaxValue)]
        public long StatusChangeTimeout
        {
            set { this["StatusChangeTimeout"] = value; }
            get { return (long)this["StatusChangeTimeout"]; }
        }

        [ConfigurationProperty("AllowedServiceCollection", IsRequired = true)]
        public AllowedServiceCollection AllowedServices
        {
            get { return (AllowedServiceCollection)this["AllowedServiceCollection"]; }
            set { this["AllowedServiceCollection"] = value; }
        }
    }

    public class AllowedServiceCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new AddService();
        }

        protected override object GetElementKey(ConfigurationElement service)
        {
            return ((AddService)service).Name;
        }

        public bool GetService(string serviceName, out AddService outService)
        {
            foreach (var item in Configurator.Settings.Services.AllowedServices)
            {
                AddService service = (AddService)item;
                if (serviceName == service.Name)
                {
                    outService = service;
                    return true;
                }
            }
            outService = null;
            return false;
        }
    }

    public class AddService : ConfigurationElement
    {
        [ConfigurationProperty("Name", IsKey=true, IsRequired=true)]
        public string Name 
        {
            get { return (string) base["Name"]; }
            set { base["Name"] = value; }
        }

        [ConfigurationProperty("AllowStart", IsKey = true, IsRequired = true)]
        public bool AllowStart
        {
            get { return (bool)base["AllowStart"]; }
            set { base["AllowStop"] = value; }
        }

        [ConfigurationProperty("AllowStop", IsKey = true, IsRequired = true)]
        public bool AllowStop
        {
            get { return (bool)base["AllowStop"]; }
            set { base["AllowStop"] = value; }
        }

        [ConfigurationProperty("AllowStatusCheck", IsKey = true, IsRequired = true)]
        public bool AllowStatusCheck
        {
            get { return (bool)base["AllowStatusCheck"]; }
            set { base["AllowStop"] = value; }
        }
    }

    #endregion

    #region Files

    public class Files : ConfigurationElement
    {
        [ConfigurationProperty("AllowedFileCollection", IsRequired = true)]
        public AllowedFileCollection AllowedFiles
        {
            get { return (AllowedFileCollection)this["AllowedFileCollection"]; }
            set { this["AllowedFileCollection"] = value; }
        }
    }

    public class AllowedFileCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new AddFile();
        }

        protected override object GetElementKey(ConfigurationElement service)
        {
            return ((AddFile)service).FullPath;
        }

        public bool GetFile(string alias, out AddFile outFile)
        {
            foreach (var item in Configurator.Settings.Files.AllowedFiles)
            {
                AddFile file = (AddFile)item;
                if (alias == file.Alias)
                {
                    outFile = file;
                    return true;
                }
            }
            outFile = null;
            return false;
        }
    }

    public class AddFile : ConfigurationElement
    {
        [ConfigurationProperty("FullPath", IsKey = true, IsRequired = true)]
        public string FullPath
        {
            get { return (string)base["FullPath"]; }
            set { base["FullPath"] = value; }
        }

        [ConfigurationProperty("AllowRead", IsKey = true, IsRequired = true)]
        public bool AllowRead
        {
            get { return (bool)base["AllowRead"]; }
            set { base["AllowRead"] = value; }
        }

        [ConfigurationProperty("Alias", IsKey = true, IsRequired = true)]
        public string Alias
        {
            get { return (string)base["Alias"]; }
            set { base["Alias"] = value; }
        }
    }

    #endregion

    #region Security

    public class Security : ConfigurationElement
    {
        [ConfigurationProperty("CheckAPIKey", DefaultValue = "false", IsRequired = true)]
        public bool CheckAPIKey
        {
            set { this["CheckAPIKey"] = value; }
            get { return (bool)this["CheckAPIKey"]; }
        }

        [ConfigurationProperty("CheckIPAddress", DefaultValue = "false", IsRequired = true)]
        public bool CheckIPAddress
        {
            set { this["CheckIPAddress"] = value; }
            get { return (bool)this["CheckIPAddress"]; }
        }

        [ConfigurationProperty("AllowedAPIKeyCollection", IsRequired = true)]
        public AllowedAPIKeyCollection AllowedAPIKeys
        {
            get { return (AllowedAPIKeyCollection)this["AllowedAPIKeyCollection"]; }
            set { this["AllowedAPIKeyCollection"] = value; }
        }

        [ConfigurationProperty("AllowedIPAddressCollection", IsRequired = true)]
        public AllowedIPAddressCollection AllowedIPAddresses
        {
            get { return (AllowedIPAddressCollection)this["AllowedIPAddressCollection"]; }
            set { this["AllowedIPAddressCollection"] = value; }
        }
    }

    public class AllowedAPIKeyCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new AddAPIKey();
        }

        protected override object GetElementKey(ConfigurationElement apiKey)
        {
            return ((AddAPIKey)apiKey).Value;
        }

        public bool GetAPIKey(string apiKey, out AddAPIKey outApiKey)
        {
            foreach (var item in Configurator.Settings.Security.AllowedAPIKeys)
            {
                AddAPIKey key = (AddAPIKey)item;
                if (apiKey == key.Value)
                {
                    outApiKey = key;
                    return true;
                }
            }
            outApiKey = null;
            return false;
        }
    }

    public class AddAPIKey : ConfigurationElement
    {
        [ConfigurationProperty("Value", IsKey = true, IsRequired = true)]
        public string Value
        {
            get { return (string)base["Value"]; }
            set { base["Value"] = value; }
        }
    }

    public class AllowedIPAddressCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new AddIPAddress();
        }

        protected override object GetElementKey(ConfigurationElement apiKey)
        {
            return ((AddIPAddress)apiKey).Value;
        }

        public bool GetAPIKey(string ipAddress, out AddIPAddress outIpAddress)
        {
            foreach (var item in Configurator.Settings.Security.AllowedIPAddresses)
            {
                AddIPAddress ip = (AddIPAddress)item;
                if (ipAddress == ip.Value)
                {
                    outIpAddress = ip;
                    return true;
                }
            }
            outIpAddress = null;
            return false;
        }
    }

    public class AddIPAddress : ConfigurationElement
    {
        [ConfigurationProperty("Value", IsKey = true, IsRequired = true)]
        public string Value
        {
            get { return (string)base["Value"]; }
            set { base["Value"] = value; }
        }
    }

    #endregion
}
