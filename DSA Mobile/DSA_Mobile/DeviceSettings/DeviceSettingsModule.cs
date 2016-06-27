using System.Threading;
using System.Threading.Tasks;
using DSLink.Nodes;

namespace DSAMobile.DeviceSettings
{
    public class DeviceSettingsModule : BaseModule
    {
        private readonly BaseDeviceSettings _deviceSettings;
        private CancellationTokenSource _updateToken;
        private readonly Task _updateTask;
        private Node _screenStatus;

        public bool Supported => true;

        public DeviceSettingsModule(BaseDeviceSettings deviceSettings)
        {
            _deviceSettings = deviceSettings;
            _updateToken = new CancellationTokenSource();
            _updateTask = new Task(Update, _updateToken.Token);
        }

        public bool RequestPermissions()
        {
            return true;
        }

        public void AddNodes(Node superRoot)
        {
            _screenStatus = superRoot.CreateChild("screen_status")
                     .SetDisplayName("Screen Status")
                     .SetType("bool")
                     .SetValue(_deviceSettings.ScreenOn())
                     .BuildNode();

            _updateTask.Start();
        }

        public void RemoveNodes()
        {
            _updateToken.Cancel();
            _screenStatus.RemoveFromParent();
        }

        private void Update()
        {
            while (!_updateTask.IsCanceled)
            {
                _screenStatus.Value.Set(_deviceSettings.ScreenOn());
                _updateTask.Wait(1000);
            }
        }
    }
}

