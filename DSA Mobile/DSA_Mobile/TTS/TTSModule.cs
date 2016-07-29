using System.Collections.Generic;
using DSLink.Nodes;
using DSLink.Nodes.Actions;
using DSLink.Request;
using Newtonsoft.Json.Linq;
using Plugin.TextToSpeech;

namespace DSAMobile.TTS
{
    public class TTSModule : BaseModule
    {
        private Node _speak;

        public bool Supported => true;

        public bool RequestPermissions()
        {
            return true;
        }

        public void AddNodes(Node superRoot)
        {
            CrossTextToSpeech.Current.Init();

            _speak = superRoot.CreateChild("speak")
                              .SetDisplayName("Speak")
                              .SetActionGroup(ActionGroups.Audio)
                              .AddParameter(new Parameter("text", "string"))
                              .AddParameter(new Parameter("queue", "bool", false))
                              .SetInvokable(Permission.Read)
                              .SetAction(new ActionHandler(Permission.Read, Speak))
                              .BuildNode();
        }

        public void Start()
        {
        }

        public void Stop()
        {
            _speak.RemoveFromParent();
        }

        public void Speak(InvokeRequest request)
        {
            var text = request.Parameters["text"].Value<string>();
            var queue = request.Parameters["queue"].Value<bool>();

            CrossTextToSpeech.Current.Speak(text, queue);
        }
    }
}

