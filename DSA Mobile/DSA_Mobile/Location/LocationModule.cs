using DSLink.Nodes;
using DSLink.Nodes.Actions;
using DSLink.Request;
using Plugin.ExternalMaps;
using Permission = DSLink.Nodes.Permission;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Newtonsoft.Json.Linq;

namespace DSAMobile.Location
{
    public class LocationModule : BaseModule
    {
        private Node _navigateToCoordinate;
        private Node _locLongitude;
        private Node _locLatitude;

        public bool Supported => true;

        public bool RequestPermissions()
        {
            return true;
        }

        public void AddNodes(Node superRoot)
        {
            _navigateToCoordinate = superRoot.CreateChild("nav_to_coord")
                                             .SetDisplayName("Navigate to Coordinate")
                                             .SetActionGroup(ActionGroups.Geolocation)
                                             .AddParameter(new Parameter("Name", "string"))
                                             .AddParameter(new Parameter("Latitude", "number"))
                                             .AddParameter(new Parameter("Longitude", "number"))
                                             .SetAction(new ActionHandler(Permission.Write, NavigateToCoord))
                                             .BuildNode();

            if (CrossGeolocator.Current.IsGeolocationAvailable)
            {
                _locLatitude = superRoot.CreateChild("latitude")
                                        .SetDisplayName("Latitude")
                                        .SetType(ValueType.Number)
                                        .BuildNode();

                _locLongitude = superRoot.CreateChild("longitude")
                                         .SetDisplayName("Longitude")
                                         .SetType(ValueType.Number)
                                         .BuildNode();
            }
        }

        public void Start()
        {
            if (CrossGeolocator.Current.IsGeolocationAvailable && !CrossGeolocator.Current.IsListening)
            {
                CrossGeolocator.Current.PositionChanged += LocationUpdated;
                CrossGeolocator.Current.StartListeningAsync(60, 100, false).Wait();
            }
        }

        public void Stop()
        {
            if (CrossGeolocator.Current.IsGeolocationAvailable && CrossGeolocator.Current.IsListening)
            {
                CrossGeolocator.Current.PositionChanged -= LocationUpdated;
                CrossGeolocator.Current.StopListeningAsync().Wait();
            }
        }

        public void NavigateToCoord(InvokeRequest request)
        {
            string name = request.Parameters["Name"].Value<string>();
            double longitude = request.Parameters["Longitude"].Value<double>();
            double latitude = request.Parameters["Latitude"].Value<double>();

            CrossExternalMaps.Current.NavigateTo(name, latitude, longitude);
            request.Close();
        }

        public void LocationUpdated(object sender, PositionEventArgs args)
        {
            _locLatitude.Value.Set(args.Position.Latitude);
            _locLongitude.Value.Set(args.Position.Longitude);
        }
    }
}
