using DG.Tweening;
using UnityEngine;

namespace Weapons {
	[CreateAssetMenu(fileName = "Weapon X - Stats", menuName = "SO/Weapons/Weapon Stats", order = 0)]
	public class WeaponStats : ScriptableObject {

		public enum ShootType {
			Single,
			Burst,
			Shotgun,
			[InspectorName("Shotgun + Burst")] Shotgun_Burst
		}

		public enum RadiusHitType {
			Full_Damage,
			Radius_Base
		}

		[System.Serializable]
		public class ShootSettings {

			[SerializeField] public bool bounce = false;
			[SerializeField] public bool chaosMode = false;

			[SerializeField] public bool singleHit = true;
			[SerializeField] public RadiusHitType radiusHitType = RadiusHitType.Full_Damage;
			[SerializeField] public Vector2 radiusDamageMultiplier = new Vector2(1f, .5f);

			[SerializeField] public ShootType shootType = ShootType.Single;
			[SerializeField] public float burstDelay = .2f;
			[SerializeField] public int burstAmount = 3;

			[SerializeField] public int shotgunAmount = 3;
			[SerializeField] public float shotgunOffset = 0.1f;

			[SerializeField] public int shotgunAndBurstAmount = 3;
			[SerializeField] public float shotgunAndBurstDelay = .2f;
			[SerializeField] public float shotgunAndBurstOffset = 0.1f;
		}

		[Header("Main Settings")]
		[SerializeField] public int minLevel = 0;
		[SerializeField] public int maxLevel = 0;
		[SerializeField] public Global.AudioKey shootKey = Global.AudioKey.GUN_Pistol;
		[SerializeField] public WeaponUpgradeTree tree = null;
		[SerializeField] public WeaponUpgradeSkin skin = null;
		[SerializeField] public Upgrades.UpgradePreset speedUpgrade = null;

		[Header("Settings")]
		[SerializeField] public ShootSettings shootSettings = null;

		[Header("Prefabs")]
		[SerializeField] public Bullet bulletPrefab = null;

		[Header("Spawn")]
		[SerializeField] public Vector3 spawnPosition = Vector3.zero;
		[SerializeField] public Vector3 spawnEuler = Vector3.zero;

		[Header("Camera")]
		[SerializeField] public Vector3 cameraPosition = Vector3.zero;
		[SerializeField] public float cameraSize = 9f;

		[Header("Preview")]
		[SerializeField] public Vector3 previewBy = new Vector3(10, 0, 0);
		[SerializeField] public float previewDuration = 3f;
		[SerializeField] public Ease preivewEase = Ease.InOutQuart;

		public bool ReachedNewVisualUpgrade(int currentLevel) => skin.ReachedNewVisualUpgrade(currentLevel);

		public WeaponUpgrade GetWeaponUpgrade(int currentLevel) => skin.GetWeaponUpgrade(currentLevel);
	}
}