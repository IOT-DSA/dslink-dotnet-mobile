using System.Collections.Generic;
using DSLink;

namespace DSA_Mobile
{
    public class DSLink : DSLinkContainer
    {
        private readonly App _app;
        private readonly List<BaseModule> loadedModules = new List<BaseModule>();

        public DSLink(Configuration config, App app) : base(config)
        {
			_app = app;
        }

        public void RegisterModule(BaseModule module)
        {
            var permissionsGranted = module.RequestPermissions();
            if (permissionsGranted)
            {
                module.AddNodes(Responder.SuperRoot);
                loadedModules.Add(module);
            }
        }
    }
}
