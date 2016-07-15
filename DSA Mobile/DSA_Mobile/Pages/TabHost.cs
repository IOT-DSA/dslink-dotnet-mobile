using Xamarin.Forms;

namespace DSAMobile.Pages
{
    public class TabHost : TabbedPage
    {
        public readonly ResponderPage ResponderPage;
        public readonly NavigationPage RequesterNavigation;
        public readonly RequesterPage RequesterPage;
        public readonly SettingsPage SettingsPage;

        public TabHost()
        {
            ResponderPage = new ResponderPage();
            RequesterPage = new RequesterPage();
            RequesterNavigation = new NavigationPage(RequesterPage)
            {
                Title = "Requester"
            };
            SettingsPage = new SettingsPage();

            Children.Add(ResponderPage);
            Children.Add(RequesterNavigation);
            Children.Add(SettingsPage);
        }
    }
}
