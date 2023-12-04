using UnityEngine;
using UnityEngine.UI;
using Weapons;

namespace UI {
	public class ChoicePanel : MonoBehaviour {

		[Header("Comnponents")]
		[SerializeField] private Image backgroundImage = null;
		[SerializeField] private Image foregroundImage = null;
		[SerializeField] private GameObject selectedMark = null;
		[SerializeField] private Button button = null;

		public void Initialize(UpgradeScreen upgradeScreen, WeaponChoices weaponChoices, int choiceIndex) {
			backgroundImage.color = weaponChoices.secondColor;
			foregroundImage.color = weaponChoices.firstColor;
			ModifyMark(false);

			button.onClick.AddListener(() => {
				upgradeScreen.SelectUpgrade(weaponChoices, this, choiceIndex);
			});
		}

		public void Select() {
			ModifyMark(true);
		}

		public void Deselect() {
			ModifyMark(false);
		}

		private void ModifyMark(bool value) {
			selectedMark.SetActive(value);
		}
	}
}