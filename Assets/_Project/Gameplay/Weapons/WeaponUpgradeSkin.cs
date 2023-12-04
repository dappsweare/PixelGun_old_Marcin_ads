using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Weapons {
	[CreateAssetMenu(fileName = "Weapon X - Skins", menuName = "SO/Weapons/Weapon Upgrade Skin", order = 2)]
	public class WeaponUpgradeSkin : ScriptableObject {

		[Header("Skin")]
		[SerializeField] public WeaponStats weaponStats = null;
		[SerializeField] public List<WeaponUpgrade> upgrades = null;

		public void SaveChoices(string saveKey, int choiceIndex) {
			string key = GetKey(saveKey);
			string value = PlayerPrefs.GetString(key, string.Empty);
			value += $"[{choiceIndex}]";
			PlayerPrefs.SetString(key, value);
		}

		public List<int> GetChoices(string saveKey) {
			string key = GetKey(saveKey);
			string value = PlayerPrefs.GetString(key, string.Empty);
			return GetCurrentChoices(value);
		}

		public void ResetSave(string saveKey) {
			PlayerPrefs.DeleteKey(GetKey(saveKey));
		}

		private string GetKey(string saveKey) => $"{saveKey}_{name}";

		private List<int> GetCurrentChoices(string value) {
			List<int> choices = new List<int>();

			if (string.IsNullOrEmpty(value)) return choices;
			string[] content = value.Split('[', ']')
								.Where((x) => !string.IsNullOrEmpty(x))
								.ToArray();

			for (int a = 0; a < content.Length; a++) {
				int.TryParse(content[a], out int index);
				choices.Add(index);
			}
			return choices;
		}

		public bool ReachedNewVisualUpgrade(int currentLevel) {
			for (int a = 0; a < upgrades.Count; a++) {
				if (upgrades[a].levelTarget == currentLevel)
					return true;
			}
			return false;
		}

		public WeaponUpgrade GetWeaponUpgrade(int currentLevel) {
			for (int a = 0; a < upgrades.Count; a++) {
				if (upgrades[a].levelTarget == currentLevel)
					return upgrades[a];
			}
			return null;
		}
	
	}
}