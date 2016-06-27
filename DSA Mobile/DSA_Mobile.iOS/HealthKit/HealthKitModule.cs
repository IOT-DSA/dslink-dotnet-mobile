using System;
using DSLink.Nodes;
using HealthKit;
using UIKit;
using Foundation;
using System.Diagnostics;

namespace DSAMobile.iOS.HealthKit
{
    public class HealthKitModule : BaseModule
    {
        private HKHealthStore _healthKitStore;

        public bool Supported => UIDevice.CurrentDevice.CheckSystemVersion(8, 0);

        public HealthKitModule()
        {
            _healthKitStore = new HKHealthStore();
        }

        public bool RequestPermissions()
        {
            var stepCount = HKQuantityTypeIdentifierKey.StepCount;
            var failure = false;
            _healthKitStore.RequestAuthorizationToShare(new NSSet(new[] { stepCount }), new NSSet(), (success, error) =>
            {
                if (!success)
                {
                    failure = true;
                    Debug.WriteLine("HealthKit permissions were denied.");
                }
            });
            return !failure;
        }

        public void AddNodes(Node superRoot)
        {
        }

        public void RemoveNodes()
        {
        }
    }
}

