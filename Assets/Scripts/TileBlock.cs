using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MrRob {
	public class TileBlock : MonoBehaviour, IPointerClickHandler {

		public Point Position { get; set; }

		public void SetRevealed(bool value) {
			transform.GetChild(0).gameObject.SetActive(!value);
		}

		public void OnPointerClick(PointerEventData eventData) {
			if(eventData.button == PointerEventData.InputButton.Left) {
				GameManager.Instance.OnTileClick(Position);
			}
			else if(eventData.button == PointerEventData.InputButton.Right) {
				GameManager.Instance.OnTileRightClick(Position);
			}
		}
	}
}
