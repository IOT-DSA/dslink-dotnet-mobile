﻿using DSLink.iOS;
using Foundation;
using HockeyApp.iOS;
using Plugin.Iconize;
using Plugin.Iconize.Fonts;
using UIKit;
using ZXing.Net.Mobile.Forms.iOS;

namespace DSAMobile.iOS
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
        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
			iOSPlatform.Initialize();

            // Iconize
            Iconize.With(new IoniconsModule());
            FormsPlugin.Iconize.iOS.IconControls.Init();

            // ZXing
            Platform.Init();

            // Xamarin bootstrap
            Xamarin.Forms.Forms.Init();
            _app = new iOSApp();
			LoadApplication(_app);

            var manager = BITHockeyManager.SharedHockeyManager;
            manager.Configure("315c78e1363a425f8a44823e3dfca712");
            manager.StartManager();
            manager.Authenticator.AuthenticateInstallation();
#if DEBUG
            manager.DisableCrashManager = true;
            manager.DisableInstallTracking = true;
            manager.DisableFeedbackManager = true;
            manager.DisableMetricsManager = true;
            manager.DisableUpdateManager = true;
#endif

            return base.FinishedLaunching(uiApplication, launchOptions);
        }

        public override void DidEnterBackground(UIApplication uiApplication)
        {
            base.DidEnterBackground(uiApplication);
            if (!_app.Disabled)
            {
                _app.StopLink();
                _suspended = true;
            }
        }

        public override async void WillEnterForeground(UIApplication uiApplication)
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
