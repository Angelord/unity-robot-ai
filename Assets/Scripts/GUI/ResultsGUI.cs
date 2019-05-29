using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using MrRob.GameLogic;

namespace MrRob.GUI {
	public class ResultsGUI : MonoBehaviour {

		[SerializeField] private Text text;
		[SerializeField] private float printFrequency = 0.01f;
		[SerializeField] private GameManager manager;
		
		public void ShowResults(GameResult result) {
			StringBuilder builder = new StringBuilder();

			builder.AppendLine(result.Success ? "Success!\n" : "Failure!\n");
			builder.AppendLine(String.Format("Message : {0} \n", result.Message));
			builder.AppendLine(String.Format("Step Count : {0}", result.FrameCount));

			this.gameObject.SetActive(true);
		
			StartCoroutine(PrintResultText(builder.ToString()));
		}

		public void Hide() {
			
			StopAllCoroutines();
			
			this.gameObject.SetActive(false);
		}
		
		public void OnClick() {
			manager.Reset();
		}

		private IEnumerator PrintResultText(string resultText) {

			text.text = "";
			for (int i = 0; i < resultText.Length; i++) {
				text.text += resultText[i];
				yield return new WaitForSeconds(printFrequency);
			}
		}
	}
}
