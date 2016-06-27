using System.Collections.Generic;
using DSLink.Nodes;
using DSLink.Nodes.Actions;
using DSLink.Request;
using Plugin.ExternalMaps;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Action = DSLink.Nodes.Actions.Action;
using Permission = DSLink.Nodes.Permission;
using OSPermission = Plugin.Permissions.Abstractions.Permission;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;

namespace DSAMobile.Location
{
    public class LocationModule : BaseModule
    {
        private bool _permNavigationState;
        private Node _navigateToCoordinate;
        private Node _locLongitude;
        private Node _locLatitude;

        public LocationModule()
        {
        }

        public bool Supported => true;

        public bool RequestPermissions()
        {
            var result = CrossPermissions.Current.RequestPermissionsAsync(OSPermission.Location).Result;

            foreach (KeyValuePair<OSPermission, PermissionStatus> kp in result)
            {
                if (kp.Key == OSPermission.Location)
                {
                    // If we don't get location permission, we just won't add location nodes.
                    _permNavigationState = kp.Value == PermissionStatus.Granted;
                }
            }

            return true;
        }

        public void AddNodes(Node superRoot)
        {
            _navigateToCoordinate = superRoot.CreateChild("nav_to_coord")
                                             .SetDisplayName("Navigate to Coordinate")
                                             .AddParameter(new Parameter("Name", "string"))
                                             .AddParameter(new Parameter("Latitude", "number"))
                                             .AddParameter(new Parameter("Longitude", "number"))
                                             .SetAction(new Action(Permission.Write, NavigateToCoord))
                                             .BuildNode();

            if (_permNavigationState && CrossGeolocator.Current.IsGeolocationAvailable)
            {
                _locLatitude = superRoot.CreateChild("latitude")
                                        .SetDisplayName("Latitude")
                                        .SetType("double")
                                        .BuildNode();

                _locLongitude = superRoot.CreateChild("longitude")
                                         .SetDisplayName("Longitude")
                                         .SetType("double")
                                         .BuildNode();

                CrossGeolocator.Current.PositionChanged += LocationUpdated;
                CrossGeolocator.Current.StartListeningAsync(1, 5, false).Wait();
            }
        }

        public void RemoveNodes()
        {
            _navigateToCoordinate.RemoveFromParent();
            if (_permNavigationState && CrossGeolocator.Current.IsGeolocationAvailable)
            {
                CrossGeolocator.Current.PositionChanged -= LocationUpdated;
                CrossGeolocator.Current.StopListeningAsync().Wait();

                _locLatitude.RemoveFromParent();
                _locLongitude.RemoveFromParent();
            }
        }

        public void NavigateToCoord(Dictionary<string, Value> parameters, InvokeRequest request)
        {
            string name = parameters["Name"].Get();
            double longitude = parameters["Longitude"].Get();
            double latitude = parameters["Latitude"].Get();

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

