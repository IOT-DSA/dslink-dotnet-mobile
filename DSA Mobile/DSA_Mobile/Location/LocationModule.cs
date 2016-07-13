﻿using System.Collections.Generic;
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
                                             .SetAction(new Action(Permission.Write, NavigateToCoord))
                                             .BuildNode();

            if (CrossGeolocator.Current.IsGeolocationAvailable)
            {
                _locLatitude = superRoot.CreateChild("latitude")
                                        .SetDisplayName("Latitude")
                                        .SetType("double")
                                        .BuildNode();

                _locLongitude = superRoot.CreateChild("longitude")
                                         .SetDisplayName("Longitude")
                                         .SetType("double")
                                         .BuildNode();
            }
        }

        public void Start()
        {
            if (CrossGeolocator.Current.IsGeolocationAvailable)
            {
                CrossGeolocator.Current.PositionChanged += LocationUpdated;
                CrossGeolocator.Current.StartListeningAsync(60, 100, false).Wait();
            }
        }

        public void Stop()
        {
            if (CrossGeolocator.Current.IsGeolocationAvailable)
            {
                CrossGeolocator.Current.PositionChanged -= LocationUpdated;
                CrossGeolocator.Current.StopListeningAsync().Wait();
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

