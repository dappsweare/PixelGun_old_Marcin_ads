using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Weapons;

namespace Upgrades {
	[CreateAssetMenu(fileName = "Weapon Upgrade Preset", menuName = "SO/Upgrades/Weapon Upgrade Preset", order = 1)]
	public class WeaponUpgradePreset : UpgradePreset {

		[Header("Weapons")]
		[SerializeField] public Weapon[] weaponProgress = null;

		public override void ResetSave() {
			base.ResetSave();
			for (int a = 0; a < weaponProgress.Length; a++) {
				weaponProgress[a].stats.skin.ResetSave(GetKey());
			}
		}

		public void SaveWeapon(Weapon weapon, int choiceIndex) {
			for (int a = 0; a < weaponProgress.Length; a++) {
				if (weaponProgress[a].stats == weapon.stats)
					weaponProgress[a].stats.skin.SaveChoices(GetKey(), choiceIndex);
			}
		}

		public List<int> LoadWeaponChoices(WeaponStats stats) {
			List<int> weaponChoices = new List<int>();
			for (int a = 0; a < weaponProgress.Length; a++) {
				if (weaponProgress[a].stats == stats) {
					weaponChoices = weaponProgress[a].stats.skin.GetChoices(GetKey());
					break;
				}
			}

			return weaponChoices;
		}

		public override void ModifyLevel(int modifier) {
			base.ModifyLevel(modifier);
			var weapon = GetWeapon();

			if (Application.isPlaying) {
				//CheckVisualUpgrade(weapon);
				OnLevelUp?.Invoke();
			}
		}

		private void CheckVisualUpgrade(Weapon weapon) {
			var currentLevel = GetLevel();
			if (weapon.stats.ReachedNewVisualUpgrade(currentLevel)) {
				if (WeaponController.instance == null) return;
				WeaponController.instance.ReachedNewVisualUpgrade(currentLevel);
			}
		}


		public Weapon GetWeapon() => GetWeapon(GetWeaponIndex());

		public Weapon GetWeapon(int index) => weaponProgress[index];


		public bool HasUpgrades() {
			if (!IsMaxUpgradedWeapon()) return true;

			int weaponIndex = GetWeaponIndex();
			return !IsLastWeapon(weaponIndex);
		}

		public bool WeaponHasUpgrade(int weaponIndex) {
			WeaponStats stats = weaponProgress[weaponIndex].stats;
			if (stats.skin.upgrades == null || stats.skin.upgrades.Count <= 0)
				return false;
			return true;
		}


		public bool IsMaxUpgradedWeapon() => IsMaxUpgradedWeapon(GetWeaponIndex());

		public bool IsMaxUpgradedWeapon(int weaponIndex) {
			if (!WeaponHasUpgrade(weaponIndex)) return true;

			int currentLevel = GetLevel();
			WeaponStats stats = weaponProgress[weaponIndex].stats;

			return stats.skin.upgrades[^1].levelTarget <= currentLevel;
		}


		public int GetWeaponIndex() {
			int currentLevel = GetLevel();
			int weaponIndex = 0;
			for (int a = weaponProgress.Length - 1; a >= 0; a--) {
				if (currentLevel <= weaponProgress[a].stats.maxLevel) {
					weaponIndex = a;
				}
			}

			return weaponIndex;
		}

		public bool IsLastWeapon() => IsLastWeapon(GetWeaponIndex());

		public bool IsLastWeapon(int index) => index == weaponProgress.Length - 1;


		public float GetWeaponUpgradeProgress() {
			int currentLevel = GetLevel();
			int targetLevel = 0;
			int weaponIndex = GetWeaponIndex();
			int previousLevelValue = 0;

			var progress = weaponProgress[weaponIndex];
			var weaponStats = progress.stats;
			int upgradeIndex = 0;

			for (int a = 0; a < weaponStats.skin.upgrades.Count; a++) {
				if (weaponStats.skin.upgrades[a].levelTarget > currentLevel) {
					targetLevel = weaponStats.skin.upgrades[a].levelTarget;
					upgradeIndex = a;
					break;
				}
			}

			if (weaponIndex != 0 && upgradeIndex == 0) {
				previousLevelValue += weaponProgress[weaponIndex - 1].stats.maxLevel;
			}

			if (upgradeIndex != 0) {
				previousLevelValue += weaponStats.skin.upgrades[upgradeIndex - 1].levelTarget;
			}

			float currentValue = currentLevel - previousLevelValue;
			float targetValue = targetLevel - previousLevelValue;

			return currentValue / targetValue;
		}

		public float GetNewWeaponUpgradeProgress() {
			int currentLevel = GetLevel();
			int targetLevel = GetWeapon(GetWeaponIndex()).stats.maxLevel;
			int previousLevelValue = 0;

			if(GetWeaponIndex() != 0) {
				previousLevelValue += GetWeapon(GetWeaponIndex() - 1).stats.maxLevel + 1;
			}

			float currentValue = currentLevel - previousLevelValue;
			float targetValue = targetLevel - previousLevelValue;
			return currentValue / targetValue;
		}
	}
}