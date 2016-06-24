using System.Collections.Generic;
using DSLink;

namespace DSAMobile
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
            if (module.Supported)
            {
                Logger.Info("Attempting to request permissions for " + module);
                var permissionsGranted = module.RequestPermissions();
                if (permissionsGranted)
                {
                    Logger.Info("Permissions granted, loading " + module);
                    module.AddNodes(Responder.SuperRoot);
                    loadedModules.Add(module);
                }
            }
            else
            {
                Logger.Info(module + " is not supported");
            }
        }
    }
}
