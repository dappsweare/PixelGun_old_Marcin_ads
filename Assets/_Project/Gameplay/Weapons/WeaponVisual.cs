using System.Collections.Generic;
using UnityEngine;

namespace Weapons {
    public class WeaponVisual : MonoBehaviour {

		[Header("Components")]
		[SerializeField] private MeshRenderer weaponRenderer = null;

		public void ApplyVisualUpgrade(WeaponChoices choice) {
			for (int a = 0; a < choice.modifiers.Count; a++) {
				weaponRenderer.ModifyColor(choice.modifiers[a].meshIndex, choice.modifiers[a].meshColor);
			}
		}

		public void LoadVisualUpgrade(WeaponUpgrade[] weaponUpgrades, List<int> choices) {
			int index = 0;
			for (int a = 0; a < choices.Count; a++) {
				int weaponUpgradeIndex = index;
				int choiceIndex = choices[a];
				ApplyVisualUpgrade(weaponUpgrades[weaponUpgradeIndex].choices[choiceIndex]);
				index++;
			}
		}
	}
}