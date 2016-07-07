using System;
using Android.Content;
using Android.Graphics;
using Android.Hardware.Camera2;
using Android.Runtime;
using Android.Views;
using DSLink.Nodes;

namespace DSAMobile.Droid
{
    public class StreamModule : Java.Lang.Object, BaseModule, TextureView.ISurfaceTextureListener
    {
        private MainActivity activity;
        public bool Supported => true;

        public bool RequestPermissions()
        {
            return true;
        }

        public void AddNodes(Node superRoot)
        {
            var cm = activity.GetSystemService(Context.CameraService) as CameraManager;
            string[] cameras = cm.GetCameraIdList();
            string cameraId = cameras[0];

            
        }

        public void RemoveNodes()
        {
        }

        public void OnSurfaceTextureAvailable(SurfaceTexture surface, int width, int height)
        {
            throw new NotImplementedException();
        }

        public bool OnSurfaceTextureDestroyed(SurfaceTexture surface)
        {
            throw new NotImplementedException();
        }

        public void OnSurfaceTextureSizeChanged(SurfaceTexture surface, int width, int height)
        {
            throw new NotImplementedException();
        }

        public void OnSurfaceTextureUpdated(SurfaceTexture surface)
        {
            throw new NotImplementedException();
        }
    }
}
