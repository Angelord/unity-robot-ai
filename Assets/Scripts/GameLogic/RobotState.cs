﻿
namespace MrRob.GameLogic {
	public abstract class RobotState { 

		private Robot robot;

		protected Robot Robot { get { return robot; } }

		public RobotState(Robot robot) {
			this.robot = robot;
		}

		public virtual void Reset() {
		}

		public virtual void OnEnter() {
		}

		public virtual void OnExit() {
		}

		public virtual void Step() {
		}

		protected void Done(string message) {
			Robot.Game.End(message);
			Robot.EnterState("Idle");
		} 
	}
}