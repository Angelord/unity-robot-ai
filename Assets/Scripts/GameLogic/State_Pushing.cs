using System.Collections.Generic;
using MrRob.Pathfinding;

namespace MrRob.GameLogic {
    
    //Handles logic once the robot has discovered the cargo box. 
    //While in this state, the robot tries to push the cargo to the goal.
    public class State_Pushing : RobotState {
       
        private Traverser_Cargo traverser;

        public State_Pushing(Robot robot) : base(robot) {
            this.traverser = new Traverser_Cargo(Robot);
        }
    
        public override void OnEnter() {
            Point cargoPos = Robot.Game.Cargo.Position;
            Point goalPos = Robot.Game.GoalPosition;

            if(cargoPos == goalPos) {
                Done("Cargo reached its destination!");
                return;
            }

            Path cargoPath = Robot.Pathfinding.GetPath(cargoPos, goalPos, traverser);
            UnityEngine.Debug.Log("Cargo Path " + cargoPath);
            if(cargoPath.Exists) {

                List<Point> botPath = new List<Point>();
                for(int i = 1; i < cargoPath.Length; i++) {
                    Point dir = cargoPath[i] - cargoPath[i - 1];
                    botPath.Add(cargoPath[i - 1] - dir);
                    botPath.Add(cargoPath[i - 1]);
                }

                Path finalPath = new Path();
                finalPath.Append(botPath[0]);
                for(int i  = 0; i < botPath.Count - 1; i++) {
                    if(botPath[i].GetDistance(botPath[i + 1]) != 1) {
                        finalPath.Append(Robot.Pathfinding.GetPath(botPath[i], botPath[i + 1], Robot.Traverser));
                    }
                    else {
                        finalPath.Append(botPath[i + 1]);
                    }
                }

                UnityEngine.Debug.Log("Final path " + finalPath);

                if(Robot.Position != finalPath[0]) {
                    Path pathToStart = Robot.Pathfinding.GetPath(Robot.Position, finalPath[0], Robot.Traverser);
                    finalPath.Prepend(pathToStart);
                }

                Robot.EnterState(new State_Follow(Robot, finalPath));
                return;
            }
            
            Done("Cargo cannot be pushed to the goal!");
        }
    }
}