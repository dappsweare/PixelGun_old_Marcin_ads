using Level;
using UnityEngine;
using UnityEngine.UIElements;
using static Weapons.WeaponStats;

namespace Weapons {

	public class DamageInfo {
		public float bulletSpeed;
		public float damage;
		public bool singleHit;
		public float radius;
		public RadiusHitType radiusHitType;
		public Vector2 radiusDamageMultiplier;

		public DamageInfo(float bulletSpeed, float damage, bool singleHit, float radius = 0f, RadiusHitType radiusHitType = RadiusHitType.Full_Damage, Vector2 radiusDamageMultiplier = default) {
			this.bulletSpeed = bulletSpeed;
			this.damage = damage;
			this.singleHit = singleHit;
			this.radius = radius;
			this.radiusHitType = radiusHitType;
			this.radiusDamageMultiplier = radiusDamageMultiplier;
		}
	}

	public class WeaponDamageController : Singleton<WeaponDamageController> {

		public bool DealDamage(Collision collision, DamageInfo damageInfo) {
			if (collision.gameObject.TryGetComponent(out LevelPixel levelPixel)) {
				DealDamage(levelPixel, damageInfo);
				return true;
			}
			return false;
		}

		public bool DealDamage(RaycastHit hit, DamageInfo damageInfo) {
			if (hit.collider.TryGetComponent(out LevelPixel levelPixel)) {
				DealDamage(levelPixel, damageInfo);
				return true;
			}
			return false;
		}

		public void DealDamage(LevelPixel levelPixel, DamageInfo damageInfo) => CheckDamageInfo(levelPixel, damageInfo);

		private void CheckDamageInfo(LevelPixel levelPixel, DamageInfo damageInfo) {
			if (damageInfo.singleHit) DealDamage(levelPixel, damageInfo.damage);
			else RadiusDamage(levelPixel, damageInfo.damage, damageInfo.radius, damageInfo.radiusHitType, damageInfo.radiusDamageMultiplier);
		}

		private void DealDamage(LevelPixel levelPixel, float damage) => levelPixel.DealDamage(damage);

		private void RadiusDamage(LevelPixel levelPixel, float damage, float radius, RadiusHitType radiusHitType, Vector2 radiusDamageMultiplier) {
			int pixelX = levelPixel.coordinates.x;
			int pixelY = levelPixel.coordinates.y;

			float finalRadius = radius - 1;

			for (float x = -finalRadius; x <= finalRadius; x++) {
				for (float y = -finalRadius; y <= finalRadius; y++) {
					int xPos = Mathf.RoundToInt(x);
					int yPos = Mathf.RoundToInt(y);
					Vector3 checkPosition = new Vector3(xPos, yPos, 0);
					float magnitude = checkPosition.magnitude;

					if (magnitude > finalRadius) continue;
					LevelPixel pixel = PixelController.instance.consturctor.GetPixel(xPos + pixelX, yPos + pixelY);
					if (pixel == null) continue;

					float finalDamage = damage;
					if (radiusHitType == RadiusHitType.Radius_Base) {
						float time = UnityExtension.Remap(magnitude, 0, finalRadius, 0, 1);
						float multiplier = Mathf.Lerp(radiusDamageMultiplier.x, radiusDamageMultiplier.y, time);
						finalDamage = damage * multiplier;
					}

					DealDamage(pixel, finalDamage);
				}
			}
		}
	}
}