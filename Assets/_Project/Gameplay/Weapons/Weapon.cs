using DG.Tweening;
using UnityEngine;

namespace Weapons {
	[RequireComponent(typeof(WeaponVisual))]
	[RequireComponent(typeof(WeaponEffects))]
	[RequireComponent(typeof(WeaponShoot))]
	public class Weapon : MonoBehaviour {

		[Header("Weapons Components")]
		[SerializeField] public WeaponStats stats = null;
		[SerializeField] public WeaponVisual visual = null;
		[SerializeField] public WeaponEffects effects = null;
		[SerializeField] public WeaponShoot shoot = null;

		[Header("Transforms")]
		[SerializeField] public Transform bulletSpawnPoint = null;
		[SerializeField] public Transform weaponParentTransform = null;
		[SerializeField] public Transform weaponTransform = null;

		private bool isPreview = false;

		private void OnValidate() {
			visual = GetComponent<WeaponVisual>();
			effects = GetComponent<WeaponEffects>();
			shoot = GetComponent<WeaponShoot>();
		}

		public void EnterWeapon() {
			shoot.Initialize(stats);
			effects.PlayBreath();
		}

		public void ExitWeapon() {
			Kill();
		}

		private void Pause() {
			transform.DOPause();
			bulletSpawnPoint.DOPause();
			weaponParentTransform.DOPause();
		}

		private void Resume() {
			transform.DOPlay();
			bulletSpawnPoint.DOPlay();
			weaponParentTransform.DOPlay();
		}

		private void Kill() {
			transform.DOKill();
			bulletSpawnPoint.DOKill();
			weaponParentTransform.DOKill();
		}

		public void ShootAt(float timeMultiplier) {
			if (isPreview) return;
			shoot.ShootAt(timeMultiplier);
		}

		private void LookAt(float rotationZ) {
			if (isPreview) return;
			transform.DORotateQuaternion(Quaternion.Euler(0.0f, 0.0f, rotationZ - 90f), 1f);
		}


		public void LookAt(Vector2 pointer) {
			if (pointer == Vector2.zero) return;
			float value = UnityExtension.Remap(pointer.x, -1, 1, 0, 1);
			float rortationZ = Mathf.Lerp(135, 45, value);
			LookAt(rortationZ);
		}

		public void LookAt(Transform transformTarget) {
			Vector3 difference = transformTarget.position - transform.position;
			float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
			LookAt(rotationZ);
		}

		public void SetPreview(bool value) {
			isPreview = value;
			effects.SetPreview(value);
			if (value) {
				effects.lineRenderer.enabled = false;
				Pause();

				weaponParentTransform.localEulerAngles = Vector3.zero;
				transform.localEulerAngles = Vector3.zero;
			} else {
				effects.lineRenderer.enabled = true;
				Resume();
			}

		}
	}
}