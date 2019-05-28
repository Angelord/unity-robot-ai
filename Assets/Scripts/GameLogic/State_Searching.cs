using System.Collections;
using System.Collections.Generic;
using MrRob.Pathfinding;

namespace MrRob.GameLogic {
	public class State_Searching : RobotState {

		private List<Point> passedTiles = new List<Point>();

		public State_Searching(Robot robot) : base(robot) {
		}
		
		public override void OnEnter() {

		}

		public override void Step() {

			if(Robot.CargoFound) {
				Robot.EnterState("Pushing");
				return;
			}

			List<Point> neighbouringPnts = GridUtility.GetNeighbours(Robot.Position, Robot.Game.Width, Robot.Game.Length);

			foreach(var neighbour in neighbouringPnts) {
				if(!Robot.TileIsRevealed(neighbour) && !passedTiles.Contains(neighbour)) {
					passedTiles.Add(neighbour);
				}
			}

			Point fwdPos = Robot.Position + Robot.Orientation;
			if(neighbouringPnts.Contains(fwdPos) && !Robot.TileIsRevealed(fwdPos)) {	//Try to move forward
				Robot.TryMove();
				passedTiles.Remove(fwdPos);
				return;
			}

			foreach(var pos in neighbouringPnts) {
				if(!Robot.TileIsRevealed(pos)) {
					Point dir = pos - Robot.Position;
					Robot.Look(dir);
					return;
				}
			}

			Point nearest = FindNearestUnrevealed();
			if(nearest == Point.MINUS) {
				Done("Finished exploring map");
				return;
			}

			Path pathToNearest = Robot.Pathfinding.GetPath(Robot.Position, nearest, Robot.Traverser);
			if(!pathToNearest.Exists || pathToNearest.Length == 1) {
				Done("Finished exploring map");
				return;
			}

			passedTiles.Remove(nearest);
			Robot.EnterState(new State_Follow(Robot, pathToNearest));	//Move to the last tile we passed
		}

		private Point FindNearestUnrevealed() {
			if(passedTiles.Count == 0) {
				return Point.MINUS;
			}

			return passedTiles[passedTiles.Count - 1];
		}
	}
}
