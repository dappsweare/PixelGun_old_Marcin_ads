using Gameplay;
using Level;
using UnityEngine;
using Upgrades;

namespace UI {
    public class GameplayScreen : ScreenBase {

        [Header("Components")]
        [SerializeField] private UpgradePanel[] upgradePanels = null;
		[SerializeField] private GameObject tutorialPopout = null;

		public override ScreenType GetScreenType() => ScreenType.Gameplay;

        private protected override void OnEnableAction() {
            Players.PlayerWallet.OnMoneyModify += RefreshUpgrades;

		}

		private protected override void OnDisableAction() {
			Players.PlayerWallet.OnMoneyModify -= RefreshUpgrades;
		}

		private protected override void OnOpen() {
			tutorialPopout.gameObject.SetActive(false);
			if (!PlayerPrefs.HasKey("Tutorial")) {
				tutorialPopout.gameObject.SetActive(true);
				StartCoroutine(Invoker.WaitAndInvoke(() => tutorialPopout.gameObject.SetActive(false), 5f));
				PlayerPrefs.SetInt("Tutorial", 1);
			}

			if(PixelController.instance.pixels.Count <= 0) {
				GameplayManager.instance.Victory(0);
			}
		}

		public void RefreshUpgrades() {
			for (int a = 0; a < upgradePanels.Length; a++) {
				upgradePanels[a].Refresh();
			}
		}
	}
}