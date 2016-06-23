using System;
using DSLink.Nodes;
using Plugin.Battery;
using Plugin.Battery.Abstractions;

namespace DSA_Mobile.Battery
{
    public class BatteryModule : BaseModule
    {
        private Node _percentRemaining;
        private Node _status;
        private Node _source;

        public bool Supported => true;

        private void BatteryChanged(object sender, BatteryChangedEventArgs e)
        {
            _percentRemaining.Value.Set(e.RemainingChargePercent);
            _status.Value.Set(e.Status.ToString());
            _source.Value.Set(e.PowerSource.ToString());
        }

        public bool RequestPermissions()
        {
            return true;
        }

        public void AddNodes(Node superRoot)
        {
            _percentRemaining = superRoot.CreateChild("battery_percent")
                                         .SetDisplayName("Battery Percent")
                                         .SetType("number")
                                         .SetValue(CrossBattery.Current.RemainingChargePercent)
                                         .BuildNode();

            _status = superRoot.CreateChild("battery_status")
                               .SetDisplayName("Battery Status")
                               .SetType("enum[Charging,Discharging,Full,NotCharging,Unknown]")
                               .SetValue(CrossBattery.Current.Status.ToString())
                               .BuildNode();

            _source = superRoot.CreateChild("power_source")
                               .SetDisplayName("Power Source")
                               .SetType("enum[Battery,Ac,Usb,Wireless,Other]")
                               .SetValue(CrossBattery.Current.PowerSource.ToString())
                               .BuildNode();

            CrossBattery.Current.BatteryChanged += BatteryChanged;
        }
    }
}