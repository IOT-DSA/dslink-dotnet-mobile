using System;
namespace DSA_Mobile.iOS
{
	public class iOSApp : App
	{
		protected override string StoragePath()
		{
			return Environment.GetFolderPath(Environment.SpecialFolder.Personal);
		}
	}
}

