using UnityEngine;

namespace Upgrades {
	public class UpgradeVisualNewGunPanel : UpgradeVisualPanel {

		[Header("Components")]
		[SerializeField] private WeaponUpgradePreset weaponUpgradePreset = null;
		[SerializeField] private GameObject costGO = null;
		[SerializeField] private GameObject progressGO = null;
		[SerializeField] private SlicedFilledImage progressImage = null;

		public override void Refresh(UpgradePreset preset) {
			if (weaponUpgradePreset.IsLastWeapon()) {
				gameObject.SetActive(false);
				return;
			}

			string displayName = preset.panelDisplayName;
			float percentage = weaponUpgradePreset.GetNewWeaponUpgradeProgress();
			float cost = preset.NextLevelCost();
			bool isMaxLevel = preset.IsMaxLevel();


			titleText.text = string.Format("<b>{0}</b>", displayName);
			levelText.text = !isMaxLevel ? $"{UnityExtension.GetPercentage(percentage)}" : "MAX";
			costText.text = !isMaxLevel ? string.Format("<sprite=2> ${0}", cost) : "MAX";
			progressImage.fillAmount = percentage;
			costGO.SetActive(percentage == 1);
			progressGO.SetActive(percentage != 1);

			ColorScheme colorScheme = percentage != 1 ? inactiveScheme : !preset.CanAfford(Players.PlayerWallet.instance.GetMoney()) ? inactiveScheme : activeScheme;
			ApplyScheme(colorScheme);
		}

		public override bool CanUpgrade(UpgradePreset preset) {
			float percentage = weaponUpgradePreset.GetNewWeaponUpgradeProgress();
			if (percentage < 1) return false;
			return preset.CanAfford(Players.PlayerWallet.instance.GetMoney());
		}

		public override void OnUpgrade(UpgradePreset preset) {
			weaponUpgradePreset.LevelUp();
		}
	}
}