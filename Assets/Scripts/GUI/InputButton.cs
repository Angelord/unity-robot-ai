using UnityEngine;
using UnityEngine.UI;

namespace MrRob.GUI {
	[RequireComponent(typeof(Button))]
	public class InputButton : MonoBehaviour {

		[SerializeField] private string input;
		private Button button;

		private void Start() {
			button = GetComponent<Button>();
		}

		private void Update() {
			if (Input.GetButton(input)) {
				button.onClick.Invoke();
			}
		}
	}
}