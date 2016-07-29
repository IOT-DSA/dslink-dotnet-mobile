using DSLink.Nodes;
using Plugin.Vibrate;
using System.Collections.Generic;
using DSLink.Request;
using DSLink.Nodes.Actions;
using System.Diagnostics;
using Newtonsoft.Json.Linq;

namespace DSAMobile.Vibrate
{
    public class VibrateModule : BaseModule
    {
        private Node _vibrate;

        public bool Supported => true;

        public bool RequestPermissions()
        {
            return true;
        }

        public void AddNodes(Node superRoot)
        {
            var vibrate = superRoot.CreateChild("vibrate")
                                   .SetDisplayName("Vibrate")
                                   .SetActionGroup(ActionGroups.Notifications)
                                   .SetInvokable(Permission.Write)
                                   .SetAction(new ActionHandler(Permission.Write, Vibrate));

            if (PlatformHelper.Android)
            {
                vibrate.AddParameter(new Parameter("duration", "number"));
            }

            _vibrate = vibrate.BuildNode();
        }

        public void Start()
        {
        }

        public void Stop()
        {
        }

        private void Vibrate(InvokeRequest request)
        {
            JToken token = request.Parameters["duration"];
            if (token != null && token.Type == JTokenType.Integer)
            {
                CrossVibrate.Current.Vibration(token.Value<int>());
            }
            else
            {
                CrossVibrate.Current.Vibration();
            }
            request.Close();
        }
    }
}

