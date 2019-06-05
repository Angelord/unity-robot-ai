using System.Collections.Generic;
using MrRob.Pathfinding;

namespace MrRob.GameLogic {
	public class State_Searching : RobotState {
		
		public State_Searching(Robot robot) : base(robot) {
		}

		public override void OnEnter() {
			Robot.Exploring = true;
		}

		public override void Step() {
			if(Robot.CargoFound && !Robot.ExploreFirst) {
				Robot.Exploring = false;
				Robot.EnterState("Pushing");
				return;
			}
			
			Robot.ExpandFrontier();
			List<Point> neighbouringPnts = GridUtility.GetNeighbours(Robot.Position, Robot.Game.Width, Robot.Game.Length);
			
			Point fwdPos = Robot.Position + Robot.Orientation;
			if(neighbouringPnts.Contains(fwdPos) && !Robot.TileIsRevealed(fwdPos)) {	//Try to move forward
				Robot.TryMove();
				return;
			}

			foreach(var pos in neighbouringPnts) {
				if(!Robot.TileIsRevealed(pos)) {
					Point dir = pos - Robot.Position;
					Robot.Look(dir);
					return;
				}
			}

			Robot.Traverser.AvoidCargo = true;
			Path pathToNearest = null;
			do {
				Point nearest = Robot.FindNearestUnrevealed();
				if (nearest == Point.MINUS) {
					if (Robot.CargoFound) {
						Robot.Exploring = false;
						Robot.EnterState("Pushing");
					}
					else {
						Done("Finished exploring map");
					}

					return;
				}

				pathToNearest = Robot.Pathfinding.GetPath(Robot.Position, nearest, Robot.Traverser);
			} while (!pathToNearest.Exists);

			Robot.EnterState(new State_Follow(Robot, pathToNearest));	//Move to the last tile we passed
		}
	}
}
