using DSLink.Nodes;
using Plugin.Battery;
using Plugin.Battery.Abstractions;

namespace DSA_Mobile
{
    public class BatteryModule : BaseModule
    {
        private readonly Node _percentRemaining;
        private readonly Node _status;
        private readonly Node _source;

        public BatteryModule(Node superRoot)
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

        private void BatteryChanged(object sender, BatteryChangedEventArgs e)
        {
            _percentRemaining.Value.Set(e.RemainingChargePercent);
            _status.Value.Set(e.Status.ToString());
            _source.Value.Set(e.PowerSource.ToString());
        }
    }
}
