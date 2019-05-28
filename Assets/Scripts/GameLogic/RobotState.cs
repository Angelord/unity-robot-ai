using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MrRob.GameLogic {
	public abstract class RobotState { 

		private Robot robot;

		protected Robot Robot { get { return robot; } }

		public RobotState(Robot robot) {
			this.robot = robot;
		}

		public virtual void OnEnter() {
		}

		public virtual void OnExit() {
		}

		public virtual void Step() {
		}

		protected void Done(string message) {
			UnityEngine.Debug.Log(message);
			Robot.EnterState("Done");
		} 
	}
}