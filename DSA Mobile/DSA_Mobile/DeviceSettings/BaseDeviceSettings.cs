using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace DSAMobile.DeviceSettings
{
    /// <summary>
    /// Base device settings.
    /// </summary>
    public abstract class BaseDeviceSettings
    {
        /// <summary>
        /// App instance.
        /// </summary>
        protected App _app;

        /// <summary>
        /// True if determining if WiFi state is supported.
        /// </summary>
        public virtual bool IsWiFiStateSupported => false;

        /// <summary>
        /// True if getting information about the WiFi
        /// connection is supported.
        /// </summary>
        public virtual bool GetWiFiInfoSupported => false;

        /// <summary>
        /// True if WiFi scanning is supported.
        /// </summary>
        public virtual bool WiFiScanningSupported => false;

        /// <summary>
        /// Determines if WiFi is enabled.
        /// </summary>
        public virtual bool WiFiEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// Grabs the current WiFi information.
        /// </summary>
        public virtual WiFiInfo WiFiInfo => null;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:DSAMobile.DeviceSettings.BaseDeviceSettings"/> class.
        /// </summary>
        /// <param name="app">App instance</param>
        protected BaseDeviceSettings(App app)
        {
            _app = app;
        }

        /// <summary>
        /// Sets whether the application should request the screen to stay on.
        /// </summary>
        /// <param name="screenIdleState">Screen idle state</param>
        public abstract void SetScreenIdle(bool screenIdleState);

        /// <summary>
        /// Determines if the screen is on.
        /// </summary>
        public abstract bool ScreenOn();

        /// <summary>
        /// Scans for WiFi access points nearby.
        /// </summary>
        /// <returns>WiFi objects</returns>
        public virtual void ScanWiFiPoints(Action<List<WiFiAccessPoint>> callback)
        {
        }
    }

    public class IpAddress
    {
        public readonly int Address;

        public string AddressString
        {
            get
            {
                try
                {
                    byte[] bytes = BitConverter.GetBytes(Address);
                    var sb = new StringBuilder();
                    sb.Append(bytes[0]).Append('.');
                    sb.Append(bytes[1]).Append('.');
                    sb.Append(bytes[2]).Append('.');
                    sb.Append(bytes[3]);
                    return sb.ToString();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.StackTrace);
                    return "";
                }
            }
        }

        public IpAddress(int address)
        {
            Address = address;
        }
    }

    public class WiFiAccessPoint
    {
        public string Ssid;
        public string Bssid;
        public int SignalLevel;
        public int Frequency;
    }

    public class WiFiInfo : WiFiAccessPoint
    {
        public bool HiddenSsid;
        public int LinkSpeed;
        public IpAddress IpAddress;
    }
}
