using System;
using System.Diagnostics;
using DSA_Mobile.Motion;
using UIKit;

namespace DSA_Mobile.iOS
{
	public class iOSApp : App
	{
		private nint _backgroundTask;
		private iOSMotionImplementation _motion;

		protected override string StoragePath()
		{
			return Environment.GetFolderPath(Environment.SpecialFolder.Personal);
		}

		public override BaseMotionImplementation GetMotion()
		{
			if (_motion == null)
			{
				_motion = new iOSMotionImplementation();
			}
			return _motion;
		}
		/*
		protected override void StartLinkPlatform()
		{
			Debug.WriteLine("Starting iOS Background Task");
			_backgroundTask = UIApplication.SharedApplication.BeginBackgroundTask(() =>
			{
				Debug.WriteLine("test");
				StartLink();
			});
			FinishLongRunningTask(_backgroundTask);
		}

		protected override void StopLinkPlatform()
		{
			Debug.WriteLine("Stopping iOS Background Task");
			UIApplication.SharedApplication.EndBackgroundTask(_backgroundTask);
			StopLink();
		}*/
	}
}
