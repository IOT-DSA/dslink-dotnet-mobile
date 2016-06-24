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

