using TMPro;
using UnityEngine;
using Weapons;

namespace Upgrades {
	public class UpgradeVisualWeaponPanel : UpgradeVisualPanel {

		[Header("Components")]
		[SerializeField] private UpgradePreset speedUpgrade = null;
		[SerializeField] private GameObject weaponPanel = null;
		[SerializeField] private TMP_Text weaponProgressText = null;


		public override void Refresh(UpgradePreset preset) {
			string displayName = preset.panelDisplayName;
			int level = preset.GetLevel();
			float cost = GetCost(preset);
			bool isMaxLevel = IsMaxLevel(preset);

			titleText.text = string.Format("<b>{0}</b>", displayName);
			levelText.text = !isMaxLevel ? string.Format("Lvl. {0}", level) : string.Empty;
			costText.text = !isMaxLevel ? string.Format("<sprite=2> ${0}", cost) : "MAX";

			ColorScheme colorScheme = isMaxLevel ? maxScheme : !CanAfford(preset) ? inactiveScheme : activeScheme;
			ApplyScheme(colorScheme);

			//CheckVisualUpgrade(preset);
		}

		private protected override bool IsMaxLevel(UpgradePreset preset) {
			WeaponUpgradePreset weaponUpgrade = preset as WeaponUpgradePreset;
			return preset.GetLevel() >= weaponUpgrade.GetWeapon().stats.maxLevel;
		}

		public override bool CanUpgrade(UpgradePreset preset) {
			WeaponUpgradePreset weaponUpgrade = preset as WeaponUpgradePreset;
			int currentLevel = preset.GetLevel();
			if (currentLevel >= weaponUpgrade.GetWeapon().stats.maxLevel) return false;
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

		private void CheckVisualUpgrade(UpgradePreset preset) {
			WeaponUpgradePreset weaponUpgrade = preset as WeaponUpgradePreset;
			bool hasUpgrade = !weaponUpgrade.IsMaxUpgradedWeapon();
			weaponPanel.SetActive(hasUpgrade);

			if (!hasUpgrade)
				return;

			float progress = weaponUpgrade.GetWeaponUpgradeProgress();
			weaponProgressText.text = $"New Skin: {UnityExtension.GetPercentage(progress)}";
		}
	}
}