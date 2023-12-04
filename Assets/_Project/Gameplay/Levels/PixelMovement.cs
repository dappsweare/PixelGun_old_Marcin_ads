using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

namespace Level {
	public class PixelMovement : MonoBehaviour {

		[System.Serializable]
		private struct MoveSettings {
			public float pointA;
			public float pointB;
			public float duration;
			public Ease ease;

			public MoveSettings(float pointA, float pointB, float duration, Ease ease) {
				this.pointA = pointA;
				this.pointB = pointB;
				this.duration = duration;
				this.ease = ease;
			}
		}

		private enum MoveType {
			None,
			X,
			Y,
			Rotate_Z
		}

		[Header("Components")]
		[SerializeField] private Transform pixels = null;

		[Header("Settings")]
		[SerializeField] private MoveType moveType = MoveType.None;
		[SerializeField] private MoveSettings moveSettings = new MoveSettings();

		private void OnDrawGizmos() {
			if (pixels == null || moveType == MoveType.None) return;

			Gizmos.DrawSphere(GetPixelPosition(moveSettings.pointA), 1f);
			Gizmos.DrawSphere(GetPixelPosition(moveSettings.pointB), 1f);
		}

		private void OnDisable() {
			pixels.DOKill();
		}

		private void Start() {
			if (moveType == MoveType.None) return;
			ApplyMovement();
		}

		private void ApplyMovement() {
			Vector3 position = GetPixelPosition(moveSettings.pointA);
			pixels.transform.position = position;
			if (moveType == MoveType.X) {
				pixels.DOMoveX(moveSettings.pointB, moveSettings.duration).SetEase(moveSettings.ease).SetLoops(-1, LoopType.Yoyo);
			} else if (moveType == MoveType.Y) {
				pixels.DOMoveY(moveSettings.pointB, moveSettings.duration).SetEase(moveSettings.ease).SetLoops(-1, LoopType.Yoyo);
			} else if (moveType == MoveType.Rotate_Z) {
				Vector3 targetValue = Vector3.forward;
				pixels.eulerAngles = targetValue * moveSettings.pointA;
				pixels.DORotate(targetValue * moveSettings.pointB, moveSettings.duration).SetEase(moveSettings.ease).SetLoops(-1, LoopType.Yoyo);
			}
		}


		private Vector3 GetPixelPosition(float value) {
			Vector3 position = pixels.position;
			if (moveType == MoveType.X)
				position.x = value;
			else if (moveType == MoveType.Y)
				position.y = value;
			return position;
		}
	}
}