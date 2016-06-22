using DSLink.iOS;
using Foundation;
using UIKit;

namespace DSA_Mobile.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        private iOSApp _app;
        private bool _suspended = false;

        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
			iOSPlatform.Initialize();
            Xamarin.Forms.Forms.Init();
            _app = new iOSApp();
			LoadApplication(_app);

            return base.FinishedLaunching(app, options);
        }

        public override void DidEnterBackground(UIApplication uiApplication)
        {
            base.DidEnterBackground(uiApplication);
            _app.StopLink();
            _suspended = true;
        }

        public override void WillEnterForeground(UIApplication uiApplication)
        {
            base.WillEnterForeground(uiApplication);
            if (_suspended)
            {
                _app.StartLink();
            }
            _suspended = false;
        }
    }
}
