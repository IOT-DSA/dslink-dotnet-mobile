using DSLink;

namespace DSA_Mobile.Droid
{
    public class AndroidDSLink : DSLink
    {
        private MainActivity _mainActivity;
        private FitnessModule _fitness;

        public AndroidDSLink(Configuration config, App app, MainActivity mainActivity) : base(config, app)
        {
            _mainActivity = mainActivity;
        }
    }
}

