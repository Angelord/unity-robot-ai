using System.Collections.Generic;
using MrRob.Pathfinding;

namespace MrRob.GameLogic {
    
    //Handles logic once the robot has discovered the cargo box. 
    //While in this state, the robot tries to push the cargo to the goal.
    public class State_Pushing : RobotState {
       
        private Traverser_Cargo cargoTrav;

        public State_Pushing(Robot robot) : base(robot) {
            this.cargoTrav = new Traverser_Cargo(Robot);
        }
    
        public override void OnEnter() {
            Point cargoPos = Robot.Game.Cargo.Position;
            Point goalPos = Robot.Game.GoalPosition;

            if(cargoPos == goalPos) {
                Done("Cargo reached its destination!");
                return;
            }

            Path cargoPath = Robot.Pathfinding.GetPath(cargoPos, goalPos, cargoTrav);
            UnityEngine.Debug.Log("Cargo Path " + cargoPath);
            if(cargoPath.Exists) {

                Dictionary<int, Point> cargoPositionsAtPathPoints = new Dictionary<int, Point>();

                List<Point> botPath = new List<Point>();
                for(int i = 1; i < cargoPath.Length; i++) {
                    Point dir = cargoPath[i] - cargoPath[i - 1];
                    cargoPositionsAtPathPoints[botPath.Count] = cargoPath[i];
                    cargoPositionsAtPathPoints[botPath.Count + 1] = cargoPath[i];
                    botPath.Add(cargoPath[i - 1] - dir);
                    botPath.Add(cargoPath[i - 1]);
                }

                Path finalPath = new Path();
                finalPath.Append(botPath[0]);
                for(int i  = 0; i < botPath.Count - 1; i++) {
                    Robot.Traverser.FixedBlocks.Clear();

                    if(botPath[i].GetDistance(botPath[i + 1]) != 1) {
                        Robot.Traverser.FixedBlocks.Add(cargoPositionsAtPathPoints[i + 1]);
                        Path pathToPushPos = Robot.Pathfinding.GetPath(botPath[i], botPath[i + 1], Robot.Traverser);
                        if(!pathToPushPos.Exists) {
                            Done("Cargo cannot be pushed to goal! 1");
                            return;
                        } 
                        finalPath.Append(pathToPushPos);
                    }
                    else {
                        finalPath.Append(botPath[i + 1]);
                    }

                }

                Robot.Traverser.FixedBlocks.Clear();
                UnityEngine.Debug.Log("Final path " + finalPath);

                if(Robot.Position != finalPath[0]) {
                    Path pathToStart = Robot.Pathfinding.GetPath(Robot.Position, finalPath[0], Robot.Traverser);
                    if(!pathToStart.Exists) {
                        Done("Cargo cannot be pushed to goal : can't reach start pos!");
                        return;
                    }
                    finalPath.Prepend(pathToStart);
                }

                Robot.EnterState(new State_Follow(Robot, finalPath));
                return;
            }
            
            Done("Cargo cannot be pushed to the goal!");
        }
    }
}