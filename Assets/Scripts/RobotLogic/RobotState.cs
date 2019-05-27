using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MrRob.RobotLogic {
	public abstract class RobotState : MonoBehaviour { 

		private Robot robot;

		public void Initialize(Robot robot) {
			this.robot = robot;
		}
	}
}