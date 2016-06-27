using System.Collections.Generic;
using System.Linq;
using DSLink.Nodes;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;

namespace DSAMobile.Connectivity
{
    public class ConnectivityModule : BaseModule
    {
        private Node _root;
        private Node _connectionTypes;

        public bool Supported
        {
            get
            {
                return true;
            }
        }

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
                                    .SetType("array")
                                    .SetValue(connTypes)
                                    .BuildNode();
        }

        public void RemoveNodes()
        {
            _connectionTypes.RemoveFromParent();
            _root.RemoveFromParent();
        }

        private void ConnectivityUpdatedEvent(object sender, ConnectivityChangedEventArgs eventArgs)
        {
            _connectionTypes.Value.Set(ConnectionTypesList(CrossConnectivity.Current.ConnectionTypes));
        }

        private static List<string> ConnectionTypesList(IEnumerable<ConnectionType> types)
        {
            var ret = new List<string>();
            foreach (ConnectionType ct in types)
            {
                ret.Add(ct.ToString());
            }
            return ret;
        }
    }
}
