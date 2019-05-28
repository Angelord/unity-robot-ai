using System.Collections;
using System.Collections.Generic;

namespace MrRob.GameLogic {
	public class State_Searching : RobotState {

		public State_Searching(Robot robot) : base(robot) {
		}
		
		public override void OnEnter() {

		}

		public override void Step() {

			if(Robot.TreasureFound) {
				Robot.EnterState("Pushing");
			}

			List<Point> neighbouringPnts = GridUtility.GetNeighbours(Robot.Position, Robot.Map.Width, Robot.Map.Length);

			Point fwdPos = Robot.Position + Robot.Orientation;
			if(neighbouringPnts.Contains(fwdPos) && !Robot.TileIsRevealed(fwdPos)) {	//Try to move forward
				Robot.TryMove(fwdPos);
				return;
			}

			foreach(var pos in neighbouringPnts) {
				if(!Robot.TileIsRevealed(pos)) {
					Point dir = pos - Robot.Position;
					Robot.Look(dir);
					return;
				}
			}

			UnityEngine.Debug.Log("No more direct neighbours to move to");
			Robot.EnterState("Done");
		}

		//Find nearest unrevealed
	}
}
