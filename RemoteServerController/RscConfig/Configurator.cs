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
using System.Configuration;

namespace RscConfig
{
    public class Configurator : ConfigurationSection
    {
        private static Configurator instance;

        static Configurator()
        {
            instance = ConfigurationManager.GetSection("Configurator") as Configurator;
        }

        public static Configurator Settings
        {
            get { return instance; }
        }

        [ConfigurationProperty("GeneralSettings", IsRequired = true)]
        public GeneralSettings GeneralSettings
        {
            set { this["GeneralSettings"] = value; }
            get { return (GeneralSettings)this["GeneralSettings"]; }
        }

        [ConfigurationProperty("Services", IsRequired = false)]
        public Services Services
        {
            set { this["Services"] = value; }
            get { return (Services)this["Services"]; }
        }
    }

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

    public class GeneralSettings : ConfigurationElement
    {
        [ConfigurationProperty("DefaultTimeout", DefaultValue = "5000", IsRequired = true)]
        [LongValidator(MinValue=0, MaxValue=long.MaxValue)]
        public long DefaultTimeout
        {
            set { this["DefaultTimeout"] = value; }
            get { return (long)this["DefaultTimeout"]; }
        }
    }
}
