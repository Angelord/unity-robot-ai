using System;
using UnityEngine;

namespace MrRob.Objects {
	public class ObjectStateMachine : MonoBehaviour {

		[SerializeField] private string defaultState;
		private GameObject curState;

		public GameObject CurState {
			get { return curState; }
		}

		private void Start() {
			EnterState(defaultState);
		}

		public void EnterState(string name) {
			curState = null;
			
			for (int i = 0; i < transform.childCount; i++) {
				Transform child = transform.GetChild(i);
				if (child.name != name && child.gameObject.activeSelf) {
					child.gameObject.SetActive(false);
				}
				else if (child.name == name && !child.gameObject.activeSelf) {
					curState = child.gameObject;
					child.gameObject.SetActive(true);
				}	
			}

			if (curState == null) {
				Debug.LogWarning(String.Format("State not found : {0}.", name));
			}
		}
	}

}