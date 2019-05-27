using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MrRob.GameLogic {
	public abstract class RobotState { 

		private Robot robot;

		public RobotState(Robot robot) {
			this.robot = robot;
		}

		public virtual void OnEnter() {
		}

		public virtual void OnExit() {
		}

		public virtual void Step() {
		}
	}
}