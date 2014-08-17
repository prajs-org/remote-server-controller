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

        [ConfigurationProperty("Services", IsRequired = false)]
        public Services Services
        {
            set { this["Services"] = value; }
            get { return (Services)this["Services"]; }
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

        [ConfigurationProperty("LogLevel", DefaultValue = "Info", IsRequired = true)]
        public string LogLevel
        {
            set { this["LogLevel"] = value; }
            get { return (string)this["LogLevel"]; }
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
}
