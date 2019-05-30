using UnityEngine;

namespace MrRob.Objects {
	public class MovingObject : MonoBehaviour {

		[SerializeField] private float moveDuration = 0.5f;
		private bool moving;
		private float moveStartTime;
		private Vector3 startPos;
		private Vector3 targetPos;

		public float MoveDuration {
			get { return moveDuration; }
			set {
				//moveStartTime -= (value - moveDuration) / 2;
				moveStartTime = Time.time - Progress * value;
				moveDuration = value;
			}
		}
		private float Progress {
			get {
				return Mathf.Clamp((Time.time - moveStartTime) / moveDuration, 0.0f, 1.0f);
			}
		}
		//(Time/time - moveStartTime) / moveDuration = x
		
		public bool Moving { get { return moving; } }

		public void Move(Vector3 position) {
			moving = true;
			moveStartTime = Time.time;
			startPos = transform.position;
			targetPos = position;
		}

		public void MoveInstant(Vector3 position) {
			moving = false;
			transform.position = position;
		}

		public void Stop() {
			moving = false;
		}

		private void Update() {

			if (moving) {
				float progress = Progress;
				transform.position = Vector3.Lerp(startPos, targetPos, progress);

				if (progress >= 1.0f) {
					transform.position = targetPos;
					moving = false;
				}
			}
		}
	}
}
