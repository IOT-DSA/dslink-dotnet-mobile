namespace DSA_Mobile.Camera
{
    public abstract class BaseCamera
    {
        protected App _app;

        public BaseCamera(App app)
        {
            _app = app;
        }

        public abstract void OpenCamera();
        //public abstract string ReceiveData();
    }
}
