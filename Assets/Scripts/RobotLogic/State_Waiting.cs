using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MrRob.RobotLogic {
	public class State_Waiting : RobotState {

		private void OnEnable() {
			Debug.Log("Beep boop... Waiting for orders");
		}
	}
}
