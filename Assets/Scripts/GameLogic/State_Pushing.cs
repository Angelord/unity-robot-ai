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
            Robot.Traverser.AvoidCargo = false;
            
            Point cargoPos = Robot.Game.Cargo.Position;
            Point goalPos = Robot.Game.GoalPosition;

            if(cargoPos == goalPos) {
                Done("Cargo reached its destination!");
                return;
            }

            Path cargoPath = Robot.Pathfinding.GetPath(cargoPos, goalPos, cargoTrav);
            UnityEngine.Debug.Log("Cargo Path " + cargoPath);
            if(cargoPath.Exists) {

                List<PushSegment> pushSegments = new List<PushSegment>();
                
                for(int i = 1; i < cargoPath.Length; i++) {
                    Point dir = cargoPath[i] - cargoPath[i - 1];
                    Point from = cargoPath[i - 1] - dir;
                    Point to = cargoPath[i - 1];

                    if (!Robot.Traverser.CanTraverse(Robot.Game.GetTile(from))) {
                        Done("Cargo cannot be pushed to goal! - 1 " + from);
                        return;
                    }

                    pushSegments.Add(new PushSegment(from, to));
                }

                Path finalPath = new Path();

                if (Robot.Position != pushSegments[0].from) {
                    Robot.Traverser.FixedBlocks.Add(cargoPos);
                    Path pathToStart
                        = Robot.Pathfinding.GetPath(Robot.Position, pushSegments[0].from, Robot.Traverser);
                    
                    if (!pathToStart.Exists) {
                        Done("Cargo cannot be pushed to goal! - 2");
                        return;
                    }
                    
                    finalPath.Append(pathToStart);
                    finalPath.Append(pushSegments[0].to);
                }

                for (int i = 1; i < pushSegments.Count; i++) {
                    Robot.Traverser.FixedBlocks.Clear();
                    
                    PushSegment segment = pushSegments[i];
                    PushSegment prevSegment = pushSegments[i - 1];
                    if (prevSegment.to != segment.from) {
                        Robot.Traverser.FixedBlocks.Add(segment.to);
                        Path pathToSegment 
                            = Robot.Pathfinding.GetPath(prevSegment.to, segment.from, Robot.Traverser);
                        
                        if(!pathToSegment.Exists) {
                            Done("Cargo cannot be pushed to goal! - 2");
                            return;
                        } 
                        
                        finalPath.Append(pathToSegment);
                    }
                    
                    finalPath.Append(segment.to);
                }
                Robot.Traverser.FixedBlocks.Clear();

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
            
            Done("Cargo cannot be pushed to the goal! - 3");
        }

//        private bool LinkPositions(Path path, Point start, Point end, Point cargo) {
//            Robot.Traverser.FixedBlocks.Add(cargo);
//            Path linkingPath 
//                = Robot.Pathfinding.GetPath(start, end, Robot.Traverser);
//
//            if (!linkingPath.Exists) {
//                return false;
//            }
//
//            path.Append(linkingPath);
//            return true;
//        }

        public override void OnExit() {
            Robot.Traverser.AvoidCargo = true;
        }

        private struct PushSegment {
            public Point from;
            public Point to;

            public PushSegment(Point from, Point to) {
                this.from = from;
                this.to = to;
            }
        }
    }
}