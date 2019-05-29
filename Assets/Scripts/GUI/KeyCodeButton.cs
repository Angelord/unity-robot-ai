using UnityEngine;
using UnityEngine.UI;

namespace MrRob.GUI {
	[RequireComponent(typeof(Button))]
	public class KeyCodeButton : MonoBehaviour {

		[SerializeField] private KeyCode keyCode;
		private Button button;
		
		private void Start() {
			button = GetComponent<Button>();
		}
		
		private void Update() {
			if (Input.GetKeyDown(keyCode)) {
				button.onClick.Invoke();
			}
		}
	}
}
