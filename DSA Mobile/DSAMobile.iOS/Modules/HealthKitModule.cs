using System;
using System.Diagnostics;
using DSLink.Nodes;
using Foundation;
using HealthKit;

namespace DSAMobile.iOS.Modules
{
    public class HealthKitModule : BaseModule
    {
        private Node _healthNode;
        private HKHealthStore _healthStore;

        public HealthKitModule()
        {
            _healthStore = new HKHealthStore();
        }

        public bool Supported => HKHealthStore.IsHealthDataAvailable;

        public bool RequestPermissions()
        {
            var stepsKey = HKQuantityTypeIdentifierKey.StepCount;
            var stepsQuantityType = HKObjectType.GetQuantityType(stepsKey);

            _healthStore.RequestAuthorizationToShare(
                new NSSet(),
                new NSSet(new[] {
                    stepsQuantityType
                }),
                (success, error) =>
                {
                    if (success)
                    {
                        StepQuery();
                    }
                    else
                    {
                        Console.WriteLine(error);
                    }
                }
            );

            return true;
        }

        public void AddNodes(Node superRoot)
        {
            _healthNode = superRoot.CreateChild("healthkit")
                                   .SetDisplayName("HealthKit")
                                   .BuildNode();
        }

        public void Start()
        {
        }

        public void Stop()
        {
        }

        public int StepQuery()
        {
            var dateComponents = new NSDateComponents();
            dateComponents.Day = -1;
            var cal = new NSCalendar(NSCalendarType.ISO8601);
            var yesterday = cal.DateFromComponents(dateComponents);
            var predicate = HKQuery.GetPredicateForSamples(yesterday, new NSDate(), HKQueryOptions.None);
            var query = new HKSampleQuery(HKObjectType.GetQuantityType(HKQuantityTypeIdentifierKey.StepCount),
                                          predicate,
                                          0,
                                          null,
                                          new HKSampleQueryResultsHandler((retQuery, results, error) => {
                                              Console.WriteLine(results.Length);
            }));

            _healthStore.ExecuteQuery(query);

            return 0;
        }
    }
}
