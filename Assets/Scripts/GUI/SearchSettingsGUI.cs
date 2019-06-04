using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MrRob.GUI {
	public class SearchSettingsGUI : MonoBehaviour {

		[SerializeField] private GameManager manager;

		public void OnRunClick() {
			manager.RunSimulation();
		}
	}
}
