using System;
using System.Diagnostics;
using System.Threading.Tasks;
using DSA_Mobile.DeviceSettings;
using DSLink.Nodes;

namespace DSA_Mobile
{
    public class DeviceSettingsModule : BaseModule
    {
        private BaseDeviceSettings _deviceSettings;
        private Task _updateTask;

        private Node _screenStatus;

        public DeviceSettingsModule(Node superRoot, BaseDeviceSettings deviceSettings)
        {
            _deviceSettings = deviceSettings;
            _updateTask = new Task(Update);

            _screenStatus = superRoot.CreateChild("screen_status")
                     .SetDisplayName("Screen Status")
                     .SetType("bool")
                     .SetValue(deviceSettings.ScreenOn())
                     .BuildNode();

            _updateTask.Start();
        }

        public void Update()
        {
            while (!_updateTask.IsCanceled)
            {
                _screenStatus.Value.Set(_deviceSettings.ScreenOn());
                _updateTask.Wait(1000);
            }
        }
    }
}

