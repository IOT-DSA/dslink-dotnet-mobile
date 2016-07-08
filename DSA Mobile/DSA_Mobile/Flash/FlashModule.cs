using System;
using DSLink.Nodes;
using Lamp.Plugin;

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
                                   .SetType("bool")
                                   .SetValue(false)
                                   .BuildNode();

            _flashlight.Value.OnSet += FlashlightCallback;
        }

        public void RemoveNodes()
        {
            _flashlight.Value.OnSet -= FlashlightCallback;

            _flashlight.RemoveFromParent();
        }

        public void FlashlightCallback(Value val)
        {
            var boolState = val.Get();

            if (boolState) CrossLamp.Current.TurnOn();
            else CrossLamp.Current.TurnOff();
        }
    }
}
