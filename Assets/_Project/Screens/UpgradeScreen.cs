using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Upgrades;
using Weapons;

namespace UI {
	public class UpgradeScreen : ScreenBase {

		[Header("Components")]
		[SerializeField] private Image backgroundImage = null;
		[SerializeField] private RectTransform backgroundTransform = null;
		[SerializeField] private Button contiuneButton = null;

		[Header("Choice Panel")]
		[SerializeField] private ChoicePanel choicePanelPrefab = null;
		[SerializeField] private Transform choicePanelParent = null;
		private ChoicePanel selectedChoicePanel = null;
		private WeaponChoices selectedUpgrade = null;
		private int choiceIndex = 0;


		public override ScreenType GetScreenType() => ScreenType.Upgrade;

		private protected override void SetupScreen() {
			contiuneButton.onClick.AddListener(OnContiuneButton);
		}

		private protected override void OnOpen() {
			backgroundImage.color = Color.clear;
			backgroundImage.DOColor(new Color(0, 0, 0, .5f), .5f);

			backgroundTransform.DOAnchorPosX(1000, 0f);
			backgroundTransform.DOAnchorPosX(0, 2f).SetEase(Ease.InOutQuint);
		}

		public void Initialize(WeaponUpgrade weaponUpgrade) {
			choicePanelParent.DestroyAllChildren();
			for (int i = 0; i < weaponUpgrade.choices.Count; i++) {
				ChoicePanel choicePanel = GameObjects.GOInstantiate(choicePanelPrefab, choicePanelParent);
				choicePanel.Initialize(this, weaponUpgrade.choices[i], i);
			}
		}

		public void SelectUpgrade(WeaponChoices weaponChoices, ChoicePanel choicePanel, int choiceIndex) {
			if (selectedChoicePanel != null) {
				selectedChoicePanel.Deselect();
				selectedUpgrade = null;
			}

			selectedChoicePanel = choicePanel;
			selectedUpgrade = weaponChoices;
			this.choiceIndex = choiceIndex;
			selectedChoicePanel.Select();

			WeaponController.instance.ApplyVisualUpgrade(selectedUpgrade);
		}

		private void OnContiuneButton() {
			if (selectedChoicePanel == null) return;

			WeaponController.instance.CloseWeaponUpgrade(choiceIndex);
		}
	}
}