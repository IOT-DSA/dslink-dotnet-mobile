using DSLink.Nodes;
using DoubleRobotics;
using DSLink.Nodes.Actions;
using DSLink.Request;
using Newtonsoft.Json.Linq;

namespace DSAMobile.iOS.Modules
{
    public class DoubleRobotModule : BaseModule
    {
        private DRDouble _robot;
        private Node _robotNode;

        public bool Supported => true;

        public bool RequestPermissions()
        {
            return true;
        }

        public void AddNodes(Node superRoot)
        {
            _robot = new DRDouble();

            _robotNode = superRoot.CreateChild("robot")
                                  .SetDisplayName("Robot")
                                  .BuildNode();

            _robotNode.CreateChild("setPark")
                      .SetDisplayName("Set Park")
                      .AddParameter(new Parameter("state", "bool"))
                      .SetAction(new ActionHandler(Permission.Write, SetParkState))
                      .BuildNode();

            _robotNode.CreateChild("poleDown")
                      .SetDisplayName("Pole Down")
                      .SetAction(new ActionHandler(Permission.Write, PoleDown))
                      .BuildNode();

            _robotNode.CreateChild("poleUp")
                      .SetDisplayName("Pole Up")
                      .SetAction(new ActionHandler(Permission.Write, PoleUp))
                      .BuildNode();

            _robotNode.CreateChild("poleStop")
                      .SetDisplayName("Pole Stop")
                      .SetAction(new ActionHandler(Permission.Write, PoleStop))
                      .BuildNode();

            _robotNode.CreateChild("turnByDegrees")
                      .SetDisplayName("Turn by degrees")
                      .AddParameter(new Parameter("degrees", "number"))
                      .SetAction(new ActionHandler(Permission.Write, TurnByDegrees))
                      .BuildNode();

            _robotNode.CreateChild("drive")
                      .SetDisplayName("Drive")
                      .AddParameter(new Parameter("direction", "enum[forward,backward,stop]"))
                      .AddParameter(new Parameter("leftRight", "number"))
                      .SetAction(new ActionHandler(Permission.Write, Drive))
                      .BuildNode();
        }

        public void Start()
        {
        }

        public void Stop()
        {
        }

        public async void SetParkState(InvokeRequest request)
        {
            JToken stateToken = request.Parameters["state"];
            if (stateToken.Type == JTokenType.Boolean)
            {
                if (stateToken.Value<bool>())
                {
                    _robot.DeployKickstands();
                }
                else
                {
                    _robot.RetractKickstands();
                }
            }
            await request.Close();
        }

        public async void PoleDown(InvokeRequest request)
        {
            _robot.PoleDown();
            await request.Close();
        }

        public async void PoleUp(InvokeRequest request)
        {
            _robot.PoleUp();
            await request.Close();
        }

        public async void PoleStop(InvokeRequest request)
        {
            _robot.PoleStop();
            await request.Close();
        }

        public async void TurnByDegrees(InvokeRequest request)
        {
            var degreeToken = request.Parameters["degrees"];
            if (degreeToken.Type == JTokenType.Float)
            {
                _robot.TurnByDegrees(degreeToken.Value<float>());
            }
            await request.Close();
        }

        public async void Drive(InvokeRequest request)
        {
            var directionToken = request.Parameters["direction"];
            var leftRightToken = request.Parameters["leftRight"];
            if (directionToken.Type == JTokenType.String &&
                leftRightToken.Type == JTokenType.Float)
            {
                string direction = directionToken.Value<string>();
                float leftRight = leftRightToken.Value<float>();
                DRDriveDirection? actualDirection = null;
                switch (direction)
                {
                    case "forward":
                        actualDirection = DRDriveDirection.Forward;
                        break;
                    case "backward":
                        actualDirection = DRDriveDirection.Backward;
                        break;
                    case "stop":
                        actualDirection = DRDriveDirection.Stop;
                        break;
                }
                if (actualDirection.HasValue)
                {
                    _robot.Drive(actualDirection.Value, leftRight);
                }
            }
            await request.Close();
        }

        public async void Stop(InvokeRequest request)
        {
            _robot.Drive(DRDriveDirection.Stop, 0.0f);
            await request.Close();
        }
    }
}

