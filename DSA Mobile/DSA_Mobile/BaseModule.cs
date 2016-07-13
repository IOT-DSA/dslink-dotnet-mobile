using DSLink.Nodes;

namespace DSAMobile
{
    public interface BaseModule
    {
        bool Supported
        {
            get;
        }

        bool RequestPermissions();

        void AddNodes(Node superRoot);
        void Start();
        void Stop();
    }
}

