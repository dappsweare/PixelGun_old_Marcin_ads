using UnityEngine;
using UnityEditor;
using Weapons;

namespace Upgrades {
	[CustomEditor(typeof(WeaponUpgradePreset), true)]
	public class WeaponUpgradePresetEditor : Editor {

		private WeaponUpgradePreset preset = null;

		private void OnEnable() {
			preset = (WeaponUpgradePreset)target;
		}


		public override void OnInspectorGUI() {
			base.OnInspectorGUI();

			Dapps.EditorUtilities.HorizontalLine(5);

			DrawValues();
		}


		private void DrawValues() {
			GUILayout.Label("Weapons", Dapps.EditorUtilities.BoldLabelStyle());

			if(GUILayout.Button("Auto Sort")) {
				int currentLevel = 0;
				int repeat = 24;
				for (int a = 0; a < preset.weaponProgress.Length; a++) {
					if (preset.weaponProgress[a] == null) continue;
					int minLevel = currentLevel;
					currentLevel += repeat;
					int maxLevel = currentLevel;
					currentLevel++;

					preset.weaponProgress[a].stats.minLevel = minLevel;
					preset.weaponProgress[a].stats.maxLevel = maxLevel;
					EditorUtility.SetDirty(preset.weaponProgress[a].stats);
				}
			}

			Dapps.EditorUtilities.Vertical(() => {
				for (int a = 0; a < preset.weaponProgress.Length; a++) {
					if (preset.weaponProgress[a] == null) continue;
					DrawWeapon(preset.weaponProgress[a]);
				}
			});

		}

		private void DrawWeapon(Weapon weapon) {
			string name = weapon.name;
			Dapps.EditorUtilities.Horizontal(() => {
				GUILayout.FlexibleSpace();
				GUILayout.Label($"{name}");
				EditorGUI.BeginChangeCheck();
				weapon.stats.minLevel = EditorGUILayout.IntField(weapon.stats.minLevel, GUILayout.Width(50));
				GUILayout.Label("---");
				weapon.stats.maxLevel = EditorGUILayout.IntField(weapon.stats.maxLevel, GUILayout.Width(50));
				if (EditorGUI.EndChangeCheck()) {
					EditorUtility.SetDirty(weapon.stats);
				}
				GUILayout.FlexibleSpace();
			});
		}
	}
}