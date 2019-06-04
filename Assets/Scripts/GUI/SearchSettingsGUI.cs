using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MrRob.GUI {
	public class SearchSettingsGUI : MonoBehaviour {

		[SerializeField] private GameManager manager;
		private Toggle toggle;
		
		private void Start() {
			toggle = GetComponentInChildren<Toggle>();
		}

		private void OnEnable() {
			if (manager.Initialized) {
				toggle.isOn = manager.ExploreFirst;
			}
		}

		public void OnRunClick() {
			manager.RunSimulation();
		}

		public void OnToggleExploreFirst(bool value) {
			manager.ExploreFirst = value;
		}
	}
}
