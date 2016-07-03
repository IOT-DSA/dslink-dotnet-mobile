using System.Collections.Generic;
using DSLink;

namespace DSAMobile.Droid
{
    public class AndroidDSLink : DSLink
    {
        private MainActivity _mainActivity;
        private FitnessModule _fitness;

        public AndroidDSLink(Configuration config, App app, MainActivity mainActivity, List<BaseModule> modules) : base(config, app, modules)
        {
            _mainActivity = mainActivity;
        }
    }
}

