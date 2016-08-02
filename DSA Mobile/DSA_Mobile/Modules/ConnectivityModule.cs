using System.Collections.Generic;
using DSLink.Nodes;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;

namespace DSAMobile.Modules
{
    public class ConnectivityModule : BaseModule
    {
        private Node _root;
        private Node _connectionTypes;

        public bool Supported => true;

        public bool RequestPermissions()
        {
            return true;
        }

        public void AddNodes(Node superRoot)
        {
            var connTypes = ConnectionTypesList(CrossConnectivity.Current.ConnectionTypes);

            _root = superRoot.CreateChild("connectivity")
                             .SetDisplayName("Connectivity")
                             .BuildNode();

            _connectionTypes = _root.CreateChild("connection_types")
                                    .SetDisplayName("Types")
                                    .SetType(ValueType.Array)
                                    .SetValue(connTypes)
                                    .BuildNode();
        }

        public void Start()
        {
            CrossConnectivity.Current.ConnectivityChanged += ConnectivityUpdated;
        }

        public void Stop()
        {
            CrossConnectivity.Current.ConnectivityChanged -= ConnectivityUpdated;
        }

        private void ConnectivityUpdated(object sender, ConnectivityChangedEventArgs eventArgs)
        {
            _connectionTypes.Value.Set(new JArray(ConnectionTypesList(CrossConnectivity.Current.ConnectionTypes)));
        }

        private static JArray ConnectionTypesList(IEnumerable<ConnectionType> types)
        {
            var ret = new JArray();
            foreach (ConnectionType ct in types)
            {
                ret.Add(ct.ToString());
            }
            return ret;
        }
    }
}
