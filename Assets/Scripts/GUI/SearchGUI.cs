using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MrRob.GUI {

	public class SearchGUI : MonoBehaviour {

		[SerializeField] private Text btnText;
		[SerializeField] private GameManager manager;

		public void OnClick() {
			DoAction();
		}
		
		public void Show() {
			this.gameObject.SetActive(true);
		}

		public void Hide() {
			this.gameObject.SetActive(false);
		}
		
		private void DoAction() {
			if (manager.Replaying) {
				manager.SkipToEnd();
				btnText.text = "Run";
			}
			else {
				manager.RunSimulation();
				btnText.text = "Skip";
			}
		}
	}
}
