using System;
using DSLink.Nodes;

namespace DSA_Mobile.DeviceSettings
{
    public abstract class BaseDeviceSettings
    {
        protected App _app;

        public BaseDeviceSettings(App app)
        {
            _app = app;
        }

        public abstract bool ScreenOn();
    }
}

