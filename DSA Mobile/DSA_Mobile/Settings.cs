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

        public static string Name
        {
            get
            {
                return Get("dsamobile.name", "DSAMobile");
            }
            set
            {
                Set("dsamobile.name", value);
            }
        }

        public static string Broker
        {
            get
            {
                return Get("dsamobile.broker",
                    #if DEBUG
                    "http://localhost:8080/conn"
                    #else
                    "http://your.bro.ker/conn"
                    #endif
                );
            }
            set
            {
                Set("dsamobile.broker", value);
            }
        }

        public static dynamic Get(string key, dynamic def = null)
        {
            return _settings.GetValueOrDefault(key, def);
        }

        public static void Set(string key, dynamic value)
        {
            _settings.AddOrUpdateValue(key, value);
        }
    }
}
