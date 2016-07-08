using System.Collections.Generic;
using DSLink.Nodes;
using DSLink.Nodes.Actions;
using DSLink.Request;
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
                              .AddParameter(new Parameter("Text", "string"))
                              .AddParameter(new Parameter("Queue", "bool", false))
                              .SetInvokable(Permission.Read)
                              .SetAction(new Action(Permission.Read, Speak))
                              .BuildNode();
        }

        public void RemoveNodes()
        {
            _speak.RemoveFromParent();
        }

        public void Speak(Dictionary<string, Value> parameters, InvokeRequest request)
        {
            var text = parameters["Text"].Get();
            var queue = parameters["Queue"].Get();

            CrossTextToSpeech.Current.Speak(text, queue);
        }
    }
}

