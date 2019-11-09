using System;
using System.Configuration;
using System.Globalization;

namespace RSH.Utility
{
    public class Settings
    {
        public static string UmbracoVersion => Setting<string>("umbracoConfigurationStatus");
        public static string SummaryEmailRecipients => Setting<string>("SummaryEmailRecipients");
        public static int MaxBookingLength => Setting<int>("MaxBookingLength");

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