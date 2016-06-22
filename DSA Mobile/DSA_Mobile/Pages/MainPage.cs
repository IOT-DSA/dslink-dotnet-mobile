using Xamarin.Forms;

namespace DSA_Mobile
{
    public class MainPage : ContentPage
    {
        public MainPage()
        {
            Content = new StackLayout
            {
                Children =
                {
                    new Button
                    {
                        Text = "Start/Stop",
                        Command = new Command(() => {
                            if (App.Instance.Disabled)
                            {
                                App.Instance.StartLink();
                            }
                            else
                            {
                                App.Instance.StopLink();
                            }
                            App.Instance.Disabled = !App.Instance.Disabled;
                        })
                    }
                }
            };
        }
    }
}

