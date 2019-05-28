using MrRob.Pathfinding;

namespace MrRob.GameLogic {

    //While in this state the robot will follow a pre-determined path
    public class State_Follow : RobotState {

        private Path path;
        private int progress = 1;

        public State_Follow(Robot robot, Path path) : base(robot) {
            this.path = path;
        }

        public override void Step() {

            Point nextTile = path[progress];
            Point dir = nextTile - Robot.Position;
            if(Robot.Orientation != dir) {
                Robot.Look(dir);
                return;
            }

            if(!Robot.TryMove()) {
                Robot.ReturnToPrevState();
                return;
            }

            progress++;

            if(progress == path.Length) {
                Robot.ReturnToPrevState();
            }
        }
    }
}