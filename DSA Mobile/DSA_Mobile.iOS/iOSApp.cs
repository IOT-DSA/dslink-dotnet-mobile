using System;
using DSA_Mobile.Motion;

namespace DSA_Mobile.iOS
{
	public class iOSApp : App
	{
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
	}
}
