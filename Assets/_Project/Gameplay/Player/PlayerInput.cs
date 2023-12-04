using UnityEngine;

namespace Players {
	public class PlayerInput : Singleton<PlayerInput> {


		public static System.Action OnTripleTap = null;

		[Header("Info")]
		private int tapCount = 0;
		private float lastTimeClick;
		private float clickTime = 1f;

		private void Update() {
			//if (UnityExtension.IsOverUI()) return;

#if UNITY_EDITOR
			EditorTaps();
#else
			MobileTaps();
#endif
		}

		private void MobileTaps() {
			if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began) {
				tapCount++;
			}

			if (tapCount > 0) {
				lastTimeClick += Time.deltaTime;
			}

			if (tapCount >= 3) {
				TripleTap();
				ResetTaps();
			}

			if (lastTimeClick > clickTime) {
				ResetTaps();
			}
		}

		private void EditorTaps() {
			if (Input.GetMouseButtonDown(0)) {
				tapCount++;
			}
			if (tapCount > 0) {
				lastTimeClick += Time.deltaTime;
			}

			if (tapCount >= 3) {
				TripleTap();
				ResetTaps();
			}

			if (lastTimeClick > clickTime) {
				ResetTaps();
			}
		}

		private void TripleTap() {
			OnTripleTap?.Invoke();
		}

		private void ResetTaps() {
			lastTimeClick = 0f;
			tapCount = 0;
		}
	}
}