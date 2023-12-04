using DG.Tweening;
using UnityEngine;

namespace Weapons {
	public class WeaponEffects : MonoBehaviour {

		[System.Serializable]
		private class EaseHelper {
			public float duration;
			public Ease ease;

			public EaseHelper(float duration, Ease ease) {
				this.duration = duration;
				this.ease = ease;
			}
		}

		[System.Serializable]
		private class EaseFloat : EaseHelper {
			public float endValue;

			public EaseFloat(float endValue, float duration, Ease ease) : base(duration, ease) {
				this.endValue = endValue;
			}

		}

		[System.Serializable]
		private class EaseVector : EaseHelper {
			public Vector3 endValue;

			public EaseVector(Vector3 endValue, float duration, Ease ease) : base(duration, ease) {
				this.endValue = endValue;
			}
		}

		[Header("Weapons Components")]
		[SerializeField] private Weapon weapon = null;

		[Header("Components")]
		[SerializeField] public LineRenderer lineRenderer = null;
		[SerializeField] public GameObject newGunFX = null;
		private Transform lineRendererMarker = null;

		[Header("Breath")]
		[SerializeField] private bool onBreath = false;
		[SerializeField] private EaseFloat breathPosition = new EaseFloat(1.1f, 2f, Ease.Linear);
		[SerializeField] private EaseVector breathRotation = new EaseVector(new Vector3(-25f, 0f, 0f), 2f, Ease.Linear);

		[Header("Shoot")]
		[SerializeField] private EaseFloat shootRecoilIn = new EaseFloat(.15f, .1f, Ease.Linear);
		[SerializeField] private EaseFloat shootRecoilOut = new EaseFloat(0f, .15f, Ease.Linear);

		[Header("Preview")]
		private Vector3 eulerWeapon = Vector3.zero;

		[Header("Settings")]
		[SerializeField] private LayerMask pixelLayerMask = ~0;

		private void OnValidate() {
			weapon = GetComponent<Weapon>();
		}

		private void Start() {
			if (lineRenderer != null)
				lineRendererMarker = lineRenderer.transform.GetChild(0);
			eulerWeapon = weapon.weaponTransform.localEulerAngles;
		}

		private void Update() => UpdateLine();

		public void PlayBreath() {
			if (!onBreath) return;

			weapon.bulletSpawnPoint.DOMoveY(breathPosition.endValue, breathPosition.duration).SetEase(breathPosition.ease).SetLoops(-1, LoopType.Yoyo);
			weapon.weaponParentTransform.DOLocalRotate(breathRotation.endValue, breathRotation.duration, RotateMode.Fast).SetEase(breathRotation.ease).SetLoops(-1, LoopType.Yoyo);
		}

		public void PlayShoot() {
			weapon.weaponParentTransform.DOLocalMoveX(shootRecoilIn.endValue, shootRecoilIn.duration).SetEase(shootRecoilIn.ease);
			weapon.weaponParentTransform.DOLocalMoveX(shootRecoilOut.endValue, shootRecoilOut.duration).SetEase(shootRecoilOut.ease).SetDelay(shootRecoilIn.duration);
		}

		public void SetPreview(bool value) {
			newGunFX.SetActive(value);

			if (value) {
				weapon.weaponTransform.DOBlendableLocalRotateBy(weapon.stats.previewBy, weapon.stats.previewDuration)
					.SetEase(weapon.stats.preivewEase)
					.SetLoops(-1, LoopType.Yoyo);
			} else {
				weapon.weaponTransform.DOKill();
				weapon.weaponTransform.localEulerAngles = eulerWeapon;
			}
		}

		private void UpdateLine() {
			if (lineRenderer == null) return;

			lineRenderer.SetPosition(0, transform.position);
			if (Physics.Raycast(transform.position, transform.up, out RaycastHit hit, float.PositiveInfinity, pixelLayerMask.value)) {
				if (hit.collider) lineRenderer.SetPosition(1, hit.point);
			} else {
				lineRenderer.SetPosition(1, transform.up * 500f);
			}

			if (lineRendererMarker != null)
				lineRendererMarker.position = lineRenderer.GetPosition(1);
		}
	}
}