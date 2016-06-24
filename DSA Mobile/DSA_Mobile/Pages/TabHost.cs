using Xamarin.Forms;

namespace DSAMobile.Pages
{
    public class TabHost : TabbedPage
    {
        public TabHost()
        {
            Children.Add(new ResponderPage());
            Children.Add(new RequesterPage());
            Children.Add(new SettingsPage());
        }
    }
}
