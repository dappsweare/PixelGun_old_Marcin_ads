using UnityEngine;
using Upgrades;
using static Upgrades.UpgradePreset;

namespace Players {
    public class PlayerUpgrades : Singleton<PlayerUpgrades> {

        [Header("Components")]
        [SerializeField] public PlayerUpgrade[] upgrades = null;

		private protected override void Awake() {
			base.Awake();
			RefreshUI();
		}

		public PlayerUpgrade GetUpgrade(Upgrades.UpgradePreset.UpgradeType upgradeType) {
			for (int a = 0; a < upgrades.Length; a++) {
				if (upgrades[a].UpgradePreset().upgradeType == upgradeType)
					return upgrades[a];
			}
			return null;
        }

		public void ResetUpgrades() {
			for (int a = 0; a < upgrades.Length; a++) {
				upgrades[a].UpgradePreset().ResetSave();
			}
		}

		private void RefreshUI() {
			for (int a = 0; a < upgrades.Length; a++) {
				upgrades[a].UpgradePreset().ModifyLevel(0);
			}
		}
	}
}