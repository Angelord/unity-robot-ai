using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MrRob {
	public class TileBlock : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {

		[SerializeField] private Material defaultMat;
		[SerializeField] private Material hoverMat;
		private MeshRenderer mRenderer;
		private bool passable = true;
		private static bool dragStartedOnPassable = false;
		public Point Position { get; set; }

		private void Start() {
			mRenderer = GetComponent<MeshRenderer>();
		}

		public void SetRevealed(bool value) {
			transform.GetChild(0).gameObject.SetActive(!value);
		}

		public void SetPassable(bool value) {
			passable = value;
			mRenderer.enabled = value;
			mRenderer.material = defaultMat;
		}
		
		public void OnPointerClick(PointerEventData eventData) {
			if(eventData.button == PointerEventData.InputButton.Right) {
				GameManager.Instance.OnTileRightClick(Position);
			}
		}

		public void OnPointerEnter(PointerEventData eventData) {
			if(dragStartedOnPassable == passable && Input.GetMouseButton(0)) {
				GameManager.Instance.OnTileClick(Position);
			}
			
			mRenderer.enabled = true;
			mRenderer.material = hoverMat;
		}

		public void OnPointerExit(PointerEventData eventData) {
			mRenderer.enabled = passable;
			mRenderer.material = defaultMat;
		}

		public void OnPointerDown(PointerEventData eventData) {
			dragStartedOnPassable = passable;
			if(eventData.button == PointerEventData.InputButton.Left) {
				GameManager.Instance.OnTileClick(Position);
			}
		}
	}
}
