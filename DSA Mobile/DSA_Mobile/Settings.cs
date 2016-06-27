using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace DSAMobile
{
    public static class Settings
    {
        private static ISettings _settings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        public static string BrokerURL
        {
            get
            {
                return Get("dsamobile.broker", "http://your.bro.ker/conn");
            }
            set
            {
                Set("dsamobile.broker", value);
            }
        }

        public static void Set(string key, dynamic value)
        {
            _settings.AddOrUpdateValue(key, value);
        }

        public static dynamic Get(string key, dynamic def = null)
        {
            return _settings.GetValueOrDefault(key, def);
        }
    }
}

