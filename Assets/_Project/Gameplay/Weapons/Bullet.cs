using Level;
using UnityEngine;

namespace Weapons {
	public class Bullet : MonoBehaviour {

		[Header("Settings")]
		[SerializeField] private LayerMask raycastMask = ~0;

		[Header("Info")]
		private readonly bool selfDestroy = true;
		private bool bounce = false;
		private DamageInfo damageInfo = null;

		public void Initialize(Vector3 direction, bool bounce, bool chaosMode, DamageInfo damageInfo) {
			this.damageInfo = damageInfo;
			this.bounce = bounce;
			float offset = -90;
			if (chaosMode) {
				offset += Random.Range(0, 360);
			}
			transform.eulerAngles = UnityExtension.GetEulerByDirection(direction, offset);


			if (selfDestroy)
				Destroy(gameObject, 15f);
		}

		private void Update() {
			Move();
		}

		private void FixedUpdate() {
			CheckCollision();
		}

		private void Move() {
			transform.Translate(Vector3.up * damageInfo.bulletSpeed * Time.deltaTime);
		}

		private void CheckCollision() {
			Ray ray = new Ray(transform.position, transform.up);
			if(Physics.Raycast(ray, out RaycastHit hit, Time.fixedDeltaTime * damageInfo.bulletSpeed + .1f, raycastMask)) {
				if(hit.collider.TryGetComponent(out LevelPixel levelPixel)) {
					WeaponDamageController.instance.DealDamage(levelPixel, damageInfo);
					Destroy(gameObject);
				} else {
					if (bounce) {
						var reflect = Vector3.Reflect(ray.direction, hit.normal);
						transform.eulerAngles = UnityExtension.GetEulerByDirection(reflect, -90);
					} else {
						Destroy(gameObject);
					}
				}
			}
		}
	}
}