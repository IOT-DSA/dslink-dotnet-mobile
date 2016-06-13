using System.Diagnostics;
using DSLink.Nodes;
using Plugin.Battery;
using Plugin.Battery.Abstractions;

namespace DSA_Mobile
{
    public class BatteryModule
    {
        private readonly Node _battery;
        private readonly Node _percentRemaining;
        private readonly Node _status;
        private readonly Node _source;

        public BatteryModule(Node superRoot)
        {
            _battery = superRoot.CreateChild("Battery")
                .BuildNode();

            _percentRemaining = _battery.CreateChild("PercentRemaining")
                .SetType("number")
                .SetValue(CrossBattery.Current.RemainingChargePercent)
                .BuildNode();

            // TODO: switch to enum
            _status = _battery.CreateChild("Status")
                .SetType("string")
                .SetValue(CrossBattery.Current.Status.ToString())
                .BuildNode();

            // TODO: switch to enum
            _source = _battery.CreateChild("Source")
                .SetType("string")
                .SetValue(CrossBattery.Current.PowerSource.ToString())
                .BuildNode();

            CrossBattery.Current.BatteryChanged += BatteryChanged;
        }

        private void BatteryChanged(object sender, BatteryChangedEventArgs e)
        {
			Debug.WriteLine("Battery event");
            _percentRemaining.Value.Set(e.RemainingChargePercent);
            _status.Value.Set(e.Status.ToString());
            _source.Value.Set(e.PowerSource.ToString());
        }
    }
}
