using Xamarin.Forms;

namespace DSAMobile.Pages
{
    public class TabHost : TabbedPage
    {
        public readonly ResponderPage ResponderPage;
        public readonly RequesterPage RequesterPage;
        public readonly SettingsPage SettingsPage;

        public TabHost()
        {
            ResponderPage = new ResponderPage();
            RequesterPage = new RequesterPage();
            SettingsPage = new SettingsPage();

            Children.Add(ResponderPage);
            Children.Add(new NavigationPage(RequesterPage)
            {
                Title = "Requester"
            });
            Children.Add(SettingsPage);
        }
    }
}
