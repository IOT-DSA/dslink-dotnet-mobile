using System;
using System.Collections.Generic;
using System.Diagnostics;
using DSLink.Nodes;
using DSLink.Nodes.Actions;
using Plugin.Media;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Action = DSLink.Nodes.Actions.Action;
using Permission = DSLink.Nodes.Permission;
using OSPermission = Plugin.Permissions.Abstractions.Permission;
using DSLink.Request;
using Plugin.Media.Abstractions;

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
            foreach (KeyValuePair<OSPermission, PermissionStatus> kp in result)
            {
                Debug.WriteLine(kp.Value.ToString());
            }
            return true;
        }

        public void AddNodes(Node superRoot)
        {
            var _media = CrossMedia.Current;

            if (_media.IsTakePhotoSupported)
            {
                _takePicture = superRoot.CreateChild("take_picture")
                         .SetDisplayName("Take Picture")
                         .SetInvokable(Permission.Write)
                         .AddColumn(new Column("Data", "binary"))
                         .SetAction(new Action(Permission.Write, TakePicture))
                         .BuildNode();
            }

            if (_media.IsPickPhotoSupported)
            {
                _pickPicture = superRoot.CreateChild("pick_picture")
                                        .SetDisplayName("Pick Picture")
                                        .SetInvokable(Permission.Write)
                                        .AddColumn(new Column("Data", "binary"))
                                        .SetAction(new Action(Permission.Write, PickPicture))
                                        .BuildNode();
            }
        }

        public async void TakePicture(Dictionary<string, Value> parameters, InvokeRequest request)
        {
            var result = await CrossMedia.Current.TakePhotoAsync(
                new StoreCameraMediaOptions
                {
                    Name = "DSAPicture.jpg",
                    Directory = "Pictures"
                }
            );
            request.SendUpdates(
                new List<dynamic>
                {
                    new List<dynamic>
                    {
                        result.GetStream().ReadAllBytes()
                    }
                }
            );
            request.Close();
        }

        public async void PickPicture(Dictionary<string, Value> parameters, InvokeRequest request)
        {
            var result = await CrossMedia.Current.PickPhotoAsync();
            request.SendUpdates(
                new List<dynamic>
                {
                    new List<dynamic>
                    {
                        result.GetStream().ReadAllBytes()
                    }
                }
            );
            request.Close();
        }
    }
}

