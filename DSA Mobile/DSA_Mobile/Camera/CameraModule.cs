using System.Collections.Generic;
using DSLink.Nodes;
using DSLink.Nodes.Actions;
using Plugin.Media;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Permission = DSLink.Nodes.Permission;
using OSPermission = Plugin.Permissions.Abstractions.Permission;
using DSLink.Request;
using Plugin.Media.Abstractions;
using Newtonsoft.Json.Linq;

namespace DSAMobile.Camera
{
    public class CameraModule : BaseModule
    {
        private bool _successful;
        private Node _takePicture;
        private Node _pickPicture;

        public CameraModule()
        {
            _successful = CrossMedia.Current.Initialize().Result;
        }

        public bool Supported
        {
            get
            {
                return _successful && CrossMedia.Current.IsCameraAvailable;
            }
        }

        public bool RequestPermissions()
        {
            var result = CrossPermissions.Current.RequestPermissionsAsync(OSPermission.Camera).Result;
            bool granted = true;
            foreach (KeyValuePair<OSPermission, PermissionStatus> kp in result)
            {
                if (kp.Value != PermissionStatus.Granted)
                {
                    granted = false;
                }
            }
            return granted;
        }

        public void AddNodes(Node superRoot)
        {
            var _media = CrossMedia.Current;

            if (_media.IsTakePhotoSupported)
            {
                _takePicture = superRoot.CreateChild("take_picture")
                                        .SetDisplayName("Take Picture")
                                        .SetActionGroup(ActionGroups.Camera)
                                        .SetInvokable(Permission.Write)
                                        .AddColumn(new Column("Data", "binary"))
                                        .SetAction(new ActionHandler(Permission.Write, TakePicture))
                                        .BuildNode();
            }

            if (_media.IsPickPhotoSupported)
            {
                _pickPicture = superRoot.CreateChild("pick_picture")
                                        .SetDisplayName("Pick Picture")
                                        .SetActionGroup(ActionGroups.Camera)
                                        .SetInvokable(Permission.Write)
                                        .AddColumn(new Column("Data", "binary"))
                                        .SetAction(new ActionHandler(Permission.Write, PickPicture))
                                        .BuildNode();
            }
        }

        public void Start()
        {
        }

        public void Stop()
        {
        }

        private async void TakePicture(InvokeRequest request)
        {
            var result = await CrossMedia.Current.TakePhotoAsync(
                new StoreCameraMediaOptions
                {
                    Name = "DSAPicture.jpg",
                    Directory = "Pictures"
                }
            );

            if (result != null)
            {
                await request.UpdateTable(new Table
                {
                    new Row
                    {
                        result.GetStream().ReadAllBytes()
                    }
                });
            }

            await request.Close();
        }

        private async void PickPicture(InvokeRequest request)
        {
            var result = await CrossMedia.Current.PickPhotoAsync();

            if (result == null)
            {
                await request.UpdateTable(new Table
                {
                    new JArray
                    {
                        result.GetStream().ReadAllBytes()
                    }
                });
            }

            await request.Close();
        }
    }
}
