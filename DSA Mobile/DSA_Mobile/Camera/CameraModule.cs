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

namespace DSA_Mobile.Camera
{
    public class CameraModule : BaseModule
    {
        private bool _successful;
        private Node _takePicture;

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
                         .SetAction(new Action(Permission.Write, async (parameters, request) =>
                         {
                             try
                             {
                                 var result = await _media.TakePhotoAsync(
                                          new Plugin.Media.Abstractions.StoreCameraMediaOptions
                                          {
                                              Name = "DSAPicture.jpg",
                                              Directory = "Pictures"
                                          }
                                      );
                                 Debug.WriteLine("after picture taken");
                                 request.SendUpdates(new List<dynamic>
                                 {
                                     result.GetStream().ReadAllBytes()
                                 });
                                 Debug.WriteLine("after transmit");
                                 request.Close();
                                 Debug.WriteLine("after close");
                             }
                             catch (Exception e)
                             {
                                 Debug.WriteLine(e.StackTrace);
                             }
                         }))
                         .BuildNode();
            }
        }
    }
}

