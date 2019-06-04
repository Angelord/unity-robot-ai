using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MrRob.GUI {

	public class SearchGUI : MonoBehaviour {

		[SerializeField] private float speedFadeDuration = 1.0f;
		[SerializeField] private Text btnText;
		[SerializeField] private Text speedText;
		[SerializeField] private GameManager manager;
		private float speedMult = 1.0f;

		private void Start() {
			speedText.enabled = false;
		}

		private void OnDisable() {
			speedText.enabled = false;
		}

		private void Update() {
			if(Input.GetButtonDown("Speed_Decr") && manager.DecreaseSpeed()) {
				speedMult /= 2;
				ShowSpeedMultiplier();
			}
			if(Input.GetButtonDown("Speed_Incr") && manager.IncreaseSpeed()) {
				speedMult *= 2;
				ShowSpeedMultiplier();
			}
		}

		public void OnClick() {
			manager.SkipToEnd();
		}

		private void ShowSpeedMultiplier() {
			StopAllCoroutines();
			speedText.color = Color.white;
			speedText.text = speedMult + "*";
			speedText.enabled = true;
			StartCoroutine(HideSpeedText());
		}

		private IEnumerator HideSpeedText() {

			float elapsedTime = 0.0f;
			do {
				yield return 0;

				speedText.color = Color.Lerp(Color.white, new Color(1.0f, 1.0f, 1.0f, 0.0f),
					elapsedTime / speedFadeDuration);
				elapsedTime += Time.deltaTime;
			} while (elapsedTime <= speedFadeDuration);

			speedText.enabled = false;
		}
	}
}
