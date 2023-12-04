using UI;
using UnityEngine;
using Upgrades;
using Utilities;

namespace Weapons {
    public class WeaponController : Singleton<WeaponController> {

		[Header("Transforms")]
		[SerializeField] private Transform weaponParent = null;
		public Vector2 JoystickPosition { get; set; }

		[Header("Info")]
        private float timeMultiplier = 1f;
		private Weapon currentWeapon = null;
		private Coroutine timeCorutine = null;
		private PlayerUpgrade powerUpgrade = null;
		private PlayerUpgrade speedUpgrade = null;
		private Locker updateLock = null;
		private Level.LevelPixel currentClosestPixel = null;

		private void OnEnable() {
			powerUpgrade = Players.PlayerUpgrades.instance.GetUpgrade(UpgradePreset.UpgradeType.Power);
			speedUpgrade = Players.PlayerUpgrades.instance.GetUpgrade(UpgradePreset.UpgradeType.Speed);
			Players.PlayerInput.OnTripleTap += OnTripleTap;
		}

		private void OnDisable() {
			Players.PlayerInput.OnTripleTap -= OnTripleTap;
		}


		private void Start() {
			updateLock = new Locker();
			RefreshWeapon();
		}

        private void Update() {
			if (updateLock.IsLocked()) return;

            UpdateWeapons();
		}


		public Weapon GetWeapon() => currentWeapon;
								

		public void AddLock(string tag) => updateLock.AddTag(tag);

		public void RemoveLock(string tag) => updateLock.RemoveTag(tag);



		public void ReachedNewVisualUpgrade(int currentLevel) {
			AddLock("Visual Upgrade");

			ScreenManager.instance.ChangeScreen(ScreenType.Upgrade);
			WeaponUpgrade weaponUpgrade = currentWeapon.stats.GetWeaponUpgrade(currentLevel);
			currentWeapon.SetPreview(true);
			ScreenManager.instance.GetScreen<UpgradeScreen>(ScreenType.Upgrade).Initialize(weaponUpgrade);

			var weaponCamera = Gameplay.CameraManager.instance.weaponCamera;
			weaponCamera.transform.position = currentWeapon.stats.cameraPosition + new Vector3(0f, 0f, -10f);
			weaponCamera.orthographicSize = currentWeapon.stats.cameraSize;
		}

		public void CloseWeaponUpgrade(int choiceIndex) {
			RemoveLock("Visual Upgrade");
			currentWeapon.SetPreview(false);

			ScreenManager.instance.ChangeScreen(ScreenType.Gameplay);
			WeaponUpgradePreset weaponUpgradePreset = powerUpgrade.UpgradePreset() as WeaponUpgradePreset;
			weaponUpgradePreset.SaveWeapon(currentWeapon, choiceIndex);
		}

		public void ApplyVisualUpgrade(WeaponChoices choice) {
			currentWeapon.visual.ApplyVisualUpgrade(choice);
		}

		public void UnlockNewWeapon() {
			AddLock("New Weapon");
			ScreenManager.instance.ChangeScreen(ScreenType.NewGun);

			speedUpgrade.UpgradePreset().ResetSave();
			RemoveCurrentWeapon();
			RefreshWeapon();

			currentWeapon.SetPreview(true);

			var weaponCamera = Gameplay.CameraManager.instance.weaponCamera;
			weaponCamera.transform.position = currentWeapon.stats.cameraPosition + new Vector3(0f, 0f, -10f);
			weaponCamera.orthographicSize = currentWeapon.stats.cameraSize;
		}

		public void CloseNewWeapon() {
			RemoveLock("New Weapon");
			currentWeapon.SetPreview(false);
			ScreenManager.instance.ChangeScreen(ScreenType.Gameplay);
		}


		private void UpdateWeapons() {
			if (!Level.PixelController.instance.ContainsPixels() || currentWeapon == null) return;
			if (Global.GameManager.instance.joystickController) {
				currentWeapon.ShootAt(timeMultiplier);
				currentWeapon.LookAt(JoystickPosition);
			} else {
				if(currentClosestPixel == null)
					currentClosestPixel = Level.PixelController.instance.GetClosestPixel(currentWeapon.transform);

				if (currentClosestPixel == null) return;
				currentWeapon.ShootAt(timeMultiplier);
				currentWeapon.LookAt(currentClosestPixel.transform);
			}
		}

		public void SetNewWeapon() {
			if (currentWeapon != null)
				RemoveCurrentWeapon();
			RefreshWeapon();
		} 

		private void RefreshWeapon() {
			WeaponUpgradePreset weaponUpgradePreset = powerUpgrade.UpgradePreset() as WeaponUpgradePreset;
			Weapon newWeapon = weaponUpgradePreset.GetWeapon();
			WeaponStats stats = newWeapon.stats;

			currentWeapon = GameObjects.GOInstantiate(newWeapon, stats.spawnPosition, stats.spawnEuler, weaponParent);
			currentWeapon.visual.LoadVisualUpgrade(stats.skin.upgrades.ToArray(), weaponUpgradePreset.LoadWeaponChoices(stats));
			currentWeapon.EnterWeapon();
		}

		private void RemoveCurrentWeapon() {
			currentWeapon.ExitWeapon();
			Destroy(currentWeapon.gameObject);
			currentWeapon = null;

		}


		private void OnTripleTap() {
			if (timeCorutine != null)
				StopCoroutine(timeCorutine);

			timeCorutine = StartCoroutine(Invoker.WaitAndInvoke(() => {
				timeMultiplier = 1;
			}, .5f));
			timeMultiplier = 2f;
		}

	}
}