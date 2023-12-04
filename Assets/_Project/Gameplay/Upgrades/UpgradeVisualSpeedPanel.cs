using UnityEngine;
using Weapons;

namespace Upgrades {
	public class UpgradeVisualSpeedPanel : UpgradeVisualPanel {

		[Header("Components")]
		[SerializeField] private UpgradePreset speedUpgrade = null;

		public override void Refresh(UpgradePreset preset) {
			string displayName = speedUpgrade.panelDisplayName;
			int level = speedUpgrade.GetLevel();
			float cost = GetCost(preset);
			bool isMaxLevel = IsMaxLevel(preset);

			titleText.text = string.Format("<b>{0}</b>", displayName);
			levelText.text = !isMaxLevel ? string.Format("Lvl. {0}", level) : string.Empty;
			costText.text = !isMaxLevel ? string.Format("<sprite=2> ${0}", cost) : "MAX";

			ColorScheme colorScheme = isMaxLevel ? maxScheme : !CanAfford(preset) ? inactiveScheme : activeScheme;
			ApplyScheme(colorScheme);
		}

		private protected override bool IsMaxLevel(UpgradePreset preset) {
			WeaponUpgradePreset weaponUpgrade = preset as WeaponUpgradePreset;
			WeaponStats stats = weaponUpgrade.GetWeapon().stats;
			return speedUpgrade.GetLevel() >= stats.maxLevel;
		}

		public override bool CanUpgrade(UpgradePreset preset) {
			WeaponUpgradePreset weaponUpgrade = preset as WeaponUpgradePreset;
			WeaponStats stats = weaponUpgrade.GetWeapon().stats;

			int currentLevel = speedUpgrade.GetLevel();
			if (currentLevel >= stats.maxLevel) return false;
			return CanAfford(preset);
		}

		public override void OnUpgrade(UpgradePreset preset) {
			speedUpgrade.LevelUp();
			Players.PlayerWallet.instance.RemoveMoney(GetCost(preset));
		}

		private float GetCost(UpgradePreset preset) {
			WeaponUpgradePreset weaponUpgrade = preset as WeaponUpgradePreset;
			WeaponStats stats = weaponUpgrade.GetWeapon().stats;
			WeaponUpgradeTree tree = stats.tree;
			return tree.upgradeCost.GetValue();
		}

		private bool CanAfford(UpgradePreset preset) {
			return GetCost(preset) <= Players.PlayerWallet.instance.GetMoney();
		}
	}
}