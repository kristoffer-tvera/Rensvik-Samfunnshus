using System;
using System.Configuration;
using System.Globalization;

namespace RSH.Utility
{
    public class Settings
    {
        public static string UmbracoVersion => Setting<string>("umbracoConfigurationStatus");

        private static T Setting<T>(string name)
        {
            var val = ConfigurationManager.AppSettings[name];
            if (string.IsNullOrEmpty(val))
            {
                throw new ConfigurationErrorsException($"No setting named {name}");
            }

            return (T)Convert.ChangeType(val, typeof(T), CultureInfo.InvariantCulture);
        }
    }
}