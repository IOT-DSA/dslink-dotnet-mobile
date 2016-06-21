using DSLink.Nodes;
using Plugin.DeviceInfo;

namespace DSA_Mobile
{
    public class DeviceInfoModule
    {
        private readonly Node _operatingSystem;
        private readonly Node _operatingSystemVersion;
        private readonly Node _deviceModel;

        public DeviceInfoModule(Node superRoot)
        {
            superRoot.CreateChild("os")
                     .SetDisplayName("OS")
                     .SetType("string")
                     .SetValue(CrossDeviceInfo.Current.Platform.ToString());

            superRoot.CreateChild("os_ver")
                     .SetDisplayName("OS Version")
                     .SetType("string")
                     .SetValue(CrossDeviceInfo.Current.Version);

            superRoot.CreateChild("device_model")
                     .SetDisplayName("Device Model")
                     .SetType("string")
                     .SetValue(CrossDeviceInfo.Current.Model);
        }
    }
}

