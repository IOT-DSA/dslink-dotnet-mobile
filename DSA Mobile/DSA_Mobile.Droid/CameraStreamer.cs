using System;
using Android.OS;
using Android.Hardware;
using Android.Hardware.Camera2;
using ACamera = Android.Hardware.Camera;
using Android.Graphics;
using System.IO;

namespace DSAMobile.Droid
{
    /*
    public class CameraStreamer
    {
        private const string Tag = "CameraStreamer";
        private const int MessageTryStartStreaming = 0;
        private const int MessageSendPreviewFrame = 1;

        private readonly int _cameraIndex;
        private readonly bool _useFlashlight;
        private readonly int _previewSizeIndex;
        private readonly int _jpegQuality;

        private bool _running = false;
        private Looper _looper;
        private Handler _workHandler;
        private ACamera _camera;
        private int _previewFormat;
        private int _previewWidth;
        private int _previewHeight;
        private Rect _previewRect;
        private int _previewBufferSize;
        private MemoryStream _jpegOutputStream;

        private long _numFrames;
        private long lastTimestamp;

        public CameraStreamer(int cameraIndex, bool useFlashlight, int previewSizeIndex,
                              int jpegQuality)
        {
            _cameraIndex = cameraIndex;
            _useFlashlight = useFlashlight;
            _previewSizeIndex = previewSizeIndex;
            _jpegQuality = jpegQuality;
        }

        private class WorkHandler : Android.OS.Handler
        {
            private CameraStreamer _streamer;

            public WorkHandler(CameraStreamer streamer, Looper looper)
            {
                _streamer = streamer;
            }

            public override void HandleMessage(Message message)
            {
                switch (message.What)
                {
                    case MessageTryStartStreaming:
                        _streamer.TryStartStreaming();
                        break;
                    case MessageSendPreviewFrame:
                        var args = (Java.Lang.Object[])message.Obj;
                        _streamer.SendPreviewFrame((byte[])args[0], (ACamera)args[1], (Java.Lang.Long)args[2]);
                        break;
                    default:
                        throw new ArgumentException("Invalid message");
                }
            }
        }

        public void Start()
        {
            HandlerThread worker = new HandlerThread(Tag, (int)ThreadPriority.MoreFavorable);
            worker.Daemon = true;
            worker.Start();
            _looper = worker.Looper;
            _workHandler = new WorkHandler(this, _looper);
        }
    }
    */
}

