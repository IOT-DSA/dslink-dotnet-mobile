using FormsPlugin.Iconize;

namespace DSA_Mobile.Pages
{
    public class TabHost : IconTabbedPage
    {
        public TabHost()
        {
            Children.Add(new ResponderPage());
            Children.Add(new RequesterPage());
            Children.Add(new SettingsPage());
        }
    }
}

