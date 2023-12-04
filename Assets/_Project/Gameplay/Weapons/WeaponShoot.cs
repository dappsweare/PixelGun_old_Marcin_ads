using UnityEngine;
using static Weapons.WeaponStats;

namespace Weapons {
	public class WeaponShoot : MonoBehaviour {

		private enum MuzzleType {
			Disabled,
			AnimatedTexture,
			Particles
		}

		[Header("Weapons Components")]
		[SerializeField] private protected Weapon weapon = null;

		[Header("Components")]
		[SerializeField] private MuzzleType muzzleType = MuzzleType.AnimatedTexture;
		[SerializeField] private AnimatedTexture animatedTexture = null;
		[SerializeField] private ParticleSystem muzzlePartilces = null;
		[SerializeField] private bool playShell = true;
		[SerializeField] private ParticleSystem shellEffect = null;


		[Header("Info")]
		private WeaponStats weaponStats = null;
		private protected WeaponUpgradeTree weaponTree = null;
		private float currentShootSpeed = 0f;
		private bool canShoot = false;

		private void OnValidate() {
			weapon = GetComponent<Weapon>();
		}

		public void Initialize(WeaponStats weaponStats) {
			this.weaponStats = weaponStats;
			weaponTree = weaponStats.tree;
			ResetTimer();
		}

		public void ShootAt(float timeMultiplier) {
			if (currentShootSpeed <= 0) {
				if (!canShoot) return;
				canShoot = false;
				Shoot();
			} else
				currentShootSpeed -= timeMultiplier * Time.deltaTime;
		}

		private protected virtual void Shoot() {
			ShootSettings shootSettings = weaponStats.shootSettings;
			if (shootSettings.shootType == ShootType.Single) NormalShoot();
			else if (shootSettings.shootType == ShootType.Burst) BurstShoot(shootSettings);
			else if (shootSettings.shootType == ShootType.Shotgun) ShotgunShoot(shootSettings);
			else if (shootSettings.shootType == ShootType.Shotgun_Burst) ShotgunBurstShoot(shootSettings);
		}


		private protected void CreateBullet(Vector3 direction) {
			Transform bulletSpawnPoint = weapon.bulletSpawnPoint;
			Vector3 position = new Vector3(bulletSpawnPoint.position.x, bulletSpawnPoint.position.y, 0f);
			Bullet bullet = GetBullet(position, Vector3.zero);

			ShootSettings shootSettings = weapon.stats.shootSettings;
			float bulletSpeed = weaponTree.bulletSpeed.GetValue(-1);
			float damage = weaponTree.powerDamage.GetValue(-1);
			float radius = weaponTree.radius.GetValue(-1);
			bool singleHit = shootSettings.singleHit;
			RadiusHitType radiusHitType = shootSettings.radiusHitType;
			Vector2 radiusDamageMultiplier = shootSettings.radiusDamageMultiplier;
			bool bounce = shootSettings.bounce;
			bool chaosMode = shootSettings.chaosMode;

			DamageInfo damageInfo = new DamageInfo(bulletSpeed, damage, singleHit, radius, radiusHitType, radiusDamageMultiplier);
			bullet.Initialize(direction, bounce, chaosMode, damageInfo);
		}

		private protected void CreateEffects() {
			Global.AudioManager.instance.Play(weaponStats.shootKey);

			if (muzzleType == MuzzleType.AnimatedTexture && animatedTexture)
				animatedTexture.Play();
			else if (muzzleType == MuzzleType.Particles && muzzlePartilces)
				muzzlePartilces.Play();

			if (playShell) shellEffect.Emit(1);
			weapon.effects.PlayShoot();
			Global.VibrationManager.instance.Vibrate();
		}

		private protected void ResetTimer() {
			currentShootSpeed = weaponTree.shootSpeed.GetValue();
			canShoot = true;
		}

		private protected virtual Bullet GetBullet(Vector3 position, Vector3 euler) {
			Bullet bullet = GameObjects.GOInstantiate(weaponStats.bulletPrefab, "Bullet", position, euler, transform.parent);
			return bullet;
		}

		private Vector3 GetPointOnLine(Vector3 x, Vector3 y, float normalizedDistance) => x + (y - x) * normalizedDistance;


		private void NormalShoot() {
			CreateBullet(transform.up);
			CreateEffects();
			ResetTimer();
		}

		private void BurstShoot(ShootSettings shootSettings) {
			for (int i = 0; i < shootSettings.burstAmount; i++) {
				StartCoroutine(Invoker.WaitAndInvoke(() => {
					CreateBullet(transform.up);
					CreateEffects();

					if (i == shootSettings.burstAmount)
						ResetTimer();
				}, shootSettings.burstDelay * i));
			}
		}

		private void ShotgunShoot(ShootSettings shootSettings) {
			int totalBullets = shootSettings.shotgunAmount;

			if(totalBullets <= 1) {
				Debug.Log("Shootgun need at least 2! Override with 2.");
				totalBullets = 2;
			}

			Vector3 pointA = Vector3.right * -shootSettings.shotgunOffset;
			Vector3 pointB = Vector3.right * shootSettings.shotgunOffset;
			float maxSpacing = shootSettings.shotgunOffset / (totalBullets / 2);
			float totalLineDistance = Vector3.Distance(pointA, pointB);
			int numberOfPoints = Mathf.FloorToInt(totalLineDistance / maxSpacing);
			float actualSpacing = totalLineDistance / numberOfPoints;

			for (int a = 0; a < totalBullets; a++) {
				Vector3 point = GetPointOnLine(pointA, pointB, a * actualSpacing / totalLineDistance);
				Vector3 dir = transform.up + point;
				CreateBullet(dir);
			}

			CreateEffects();
			ResetTimer();
		}

		private void ShotgunBurstShoot(ShootSettings shootSettings) {
			int totalBullets = shootSettings.shotgunAndBurstAmount;

			if (totalBullets <= 1) {
				Debug.Log("Shootgun+Burst need at least 2! Override with 2.");
				totalBullets = 2;
			}

			Vector3 pointA = Vector3.right * -shootSettings.shotgunAndBurstOffset;
			Vector3 pointB = Vector3.right * shootSettings.shotgunAndBurstOffset;
			float maxSpacing = shootSettings.shotgunAndBurstOffset / (totalBullets / 2);
			float totalLineDistance = Vector3.Distance(pointA, pointB);
			int numberOfPoints = Mathf.FloorToInt(totalLineDistance / maxSpacing);
			float actualSpacing = totalLineDistance / numberOfPoints;

			for (int a = 0; a < totalBullets; a++) {
				Vector3 point = GetPointOnLine(pointA, pointB, a * actualSpacing / totalLineDistance);
				Vector3 dir = transform.up + point;
				StartCoroutine(Invoker.WaitAndInvoke(() => {
					CreateBullet(dir);
					CreateEffects();

					if (a == totalBullets) {
						ResetTimer();
					}

				}, shootSettings.shotgunAndBurstDelay * a));
			}
		}
	}
}