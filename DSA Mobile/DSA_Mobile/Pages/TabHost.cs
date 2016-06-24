using FormsPlugin.Iconize;

namespace DSAMobile.Pages
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

