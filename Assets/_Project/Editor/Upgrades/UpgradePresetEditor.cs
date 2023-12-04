using UnityEngine;
using UnityEditor;

namespace Upgrades {
	[CustomEditor(typeof(UpgradePreset), true)]
	public class UpgradePresetEditor : Editor {

		private UpgradePreset preset = null;
		private Vector2 scrollView = Vector2.zero;

		private void OnEnable() {
			preset = (UpgradePreset)target;
		}

		public override void OnInspectorGUI() {
			base.OnInspectorGUI();

			Dapps.EditorUtilities.HorizontalLine(5);

			DrawContent();
			DrawButtons();
			DrawWeaponUpgradePreset();
		}

		private void DrawContent() {
			Dapps.EditorUtilities.Horizontal(() => {
				GUILayout.FlexibleSpace();
				GUILayout.Label("LVL", GUILayout.Width(100));
				GUILayout.Label("COST", GUILayout.Width(100));
				GUILayout.Label("VALUE", GUILayout.Width(100));
				GUILayout.FlexibleSpace();
			});

			scrollView = GUILayout.BeginScrollView(scrollView, GUILayout.Height(200));
			EditorGUILayout.BeginVertical("Box");
			int max = Mathf.Clamp(preset.GetMaxLevel(), preset.defaultLevel, 200);
			for (int a = preset.defaultLevel; a <= preset.GetMaxLevel(); a++) {
				Dapps.EditorUtilities.Horizontal(() => {
					GUILayout.FlexibleSpace();
					GUILayout.Label(a.ToString(), GUILayout.Width(100));
					GUILayout.Label(preset.Cost(a).ToString(), GUILayout.Width(100));
					GUILayout.Label(preset.Value(a).ToString(), GUILayout.Width(100));
					GUILayout.FlexibleSpace();
				}, preset.GetLevel() == a);
			}
			EditorGUILayout.EndVertical();
			GUILayout.EndScrollView();

		}

		private void DrawButtons() {
			GUILayout.Label("Level", Dapps.EditorUtilities.BoldLabelStyle());
			Dapps.EditorUtilities.Horizontal(() => {
				if (GUILayout.Button("UP")) {
					preset.LevelUp();
				}

				if (GUILayout.Button("Down")) {
					preset.LevelDown();
				}
			});

			GUILayout.Label("Settings", Dapps.EditorUtilities.BoldLabelStyle());
			Dapps.EditorUtilities.Horizontal(() => {
				if (GUILayout.Button("Reset")) {
					preset.ResetSave();
				}

				if (GUILayout.Button("Copy Info")) {
					CopyInfo();
				}
			});

			void CopyInfo() {
				string info = "Level | Cost | Value\n";
				for (int a = preset.defaultLevel; a <= preset.GetMaxLevel(); a++) {
					string format = "Lvl.{0} | Cost: {1} | Value: {2}\n";
					info += string.Format(format, a, preset.Cost(a), preset.Value(a));
				}
				GUIUtility.systemCopyBuffer = info;
			}
		}

		private void DrawWeaponUpgradePreset() {
			if (preset is WeaponUpgradePreset weaponUpgradePreset) {
				GUILayout.Label("Weapon Upgrade Preset", Dapps.EditorUtilities.BoldLabelStyle());
				Dapps.EditorUtilities.Horizontal(() => {
					if (GUILayout.Button("Weapon Index")) {
						Debug.Log(weaponUpgradePreset.GetWeaponIndex());
					}

					if (GUILayout.Button("Weapon Upgrade")) {
						Debug.Log(weaponUpgradePreset.GetWeaponUpgradeProgress());
					}
				});
			}
		}
	}
}