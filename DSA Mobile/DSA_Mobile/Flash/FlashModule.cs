using DSLink.Nodes;
using Lamp.Plugin;
using Newtonsoft.Json.Linq;

namespace DSAMobile.Flash
{
    public class FlashModule : BaseModule
    {
        public bool Supported => true;

        private Node _flashlight;

        public bool RequestPermissions()
        {
            return true;
        }

        public void AddNodes(Node superRoot)
        {
            _flashlight = superRoot.CreateChild("flashlight")
                                   .SetDisplayName("Flashlight")
                                   .SetWritable(Permission.Write)
                                   .SetType(ValueType.Boolean)
                                   .SetValue(false)
                                   .BuildNode();
        }

        public void Start()
        {
            _flashlight.Value.OnSet += FlashlightCallback;
        }

        public void Stop()
        {
            _flashlight.Value.OnSet -= FlashlightCallback;
        }

        public void FlashlightCallback(Value val)
        {
            var boolState = val.Boolean;

            if (boolState)
            {
                CrossLamp.Current.TurnOn();
            }
            else
            {
                CrossLamp.Current.TurnOff();
            }
        }
    }
}
