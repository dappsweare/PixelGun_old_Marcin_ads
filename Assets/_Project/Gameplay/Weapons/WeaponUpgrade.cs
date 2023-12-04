using System.Collections.Generic;
using UnityEngine;

namespace Weapons {
	[System.Serializable]
	public class WeaponUpgrade {
		public int levelTarget = 0;

		[Header("Visual")]
		public List<WeaponChoices> choices = null;

	}

	[System.Serializable]
	public class WeaponChoices {
		[Header("UI")]
		public Color firstColor = Color.white;
		public Color secondColor = Color.white;

		[Header("Gameplay")]
		public List<WeaponModifier> modifiers = null;

		public WeaponChoices() {
			firstColor = Color.white;
			secondColor = Color.white;
			modifiers = new List<WeaponModifier>();
		}

		public WeaponChoices(WeaponChoices choices) {
			firstColor = choices.firstColor;
			secondColor = choices.secondColor;
			modifiers = choices.modifiers;
		}
	}

	[System.Serializable]
	public class WeaponModifier {
		public int meshIndex = 0;
		public Color meshColor = Color.white;
	}

}