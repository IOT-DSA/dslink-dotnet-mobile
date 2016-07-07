using System.Collections.Generic;
using DSLink;

namespace DSAMobile.Droid
{
    public class AndroidDSLink : DSLink
    {
        private readonly MainActivity _mainActivity;

        public AndroidDSLink(Configuration config, App app, MainActivity mainActivity, List<BaseModule> modules) : base(config, app, modules)
        {
            _mainActivity = mainActivity;
        }
    }
}

