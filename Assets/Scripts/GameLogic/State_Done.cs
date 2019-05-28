
namespace MrRob.GameLogic {

    public class State_Done : RobotState {
        public State_Done(Robot robot) : base(robot) {
        }
    
        public override void OnEnter() {
            Robot.Done = true;
        }

        public override void OnExit() {
            Robot.Done = false;
        }
    }
}