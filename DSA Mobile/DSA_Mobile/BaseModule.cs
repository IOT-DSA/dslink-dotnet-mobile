using System;
using DSLink.Nodes;

namespace DSA_Mobile
{
    public interface BaseModule
    {
        bool Supported
        {
            get;
        }

        bool RequestPermissions();

        void AddNodes(Node superRoot);
    }
}

