using UnityEngine;
using UnityEditor;
using System;
using Upgrades;

namespace Weapons {
	[CustomEditor(typeof(WeaponUpgradeTree), true)]
	[CanEditMultipleObjects]
	public class WeaponUpgradeTreeEditor : Editor {

		private WeaponUpgradeTree tree = null;
		private Vector2 scrollView = Vector2.zero;

		private void OnEnable() {
			tree = (WeaponUpgradeTree)target;
		}

		public override void OnInspectorGUI() {
			base.OnInspectorGUI();

			Dapps.EditorUtilities.HorizontalLine(5);

			DrawButtons();

			DrawValues();
		}

		private void DrawButtons() {
			if (tree.stats != null) {
				if (GUILayout.Button("Fill Stats")) {
					Modify();
				}
			}
		}

		private void DrawValues() {
			GUILayout.Label("Values", Dapps.EditorUtilities.BoldLabelStyle());
			scrollView = GUILayout.BeginScrollView(scrollView);

			Dapps.EditorUtilities.Horizontal(() => {
				GUILayout.FlexibleSpace();

				DrawUpgradeValue("Upgrade Cost", tree.upgradeCost);
				DrawUpgradeValue("Bullet Speed", tree.bulletSpeed);
				DrawUpgradeValue("Shoot Speed", tree.shootSpeed);
				DrawUpgradeValue("Power Damage", tree.powerDamage);
				DrawUpgradeValue("Radius", tree.radius);

				GUILayout.FlexibleSpace();
			});

			GUILayout.EndScrollView();
		}

		private void DrawUpgradeValue(string title, UpgradeValue upgradeValue) {
			if (upgradeValue == null || upgradeValue.stats == null || tree.stats == null) return;
			EditorGUILayout.BeginVertical("Box");

			GUILayout.Label(title, Dapps.EditorUtilities.BoldLabelStyle());
			var style = Dapps.EditorUtilities.LabelStyle(TextAnchor.MiddleCenter);
			style.margin = new RectOffset(0, 0, 0, 0);
			style.padding = new RectOffset(0, 0, 0, 0);

			int minLevel = tree.stats.minLevel;
			int totalAmount = tree.stats.maxLevel - tree.stats.minLevel;
			int currentLevel = ((upgradeValue.stats as WeaponStats).speedUpgrade.advancedMaxLevel.upgradePreset as WeaponUpgradePreset).GetLevel();	// god.

			for (int a = 0; a <= totalAmount; a++) {
				int level = a + minLevel;
				float value = upgradeValue.GetValueAtLevel(level);
				style.normal.textColor = currentLevel == level ? Color.green : Color.white;

				Dapps.EditorUtilities.Horizontal(() => {
					GUILayout.FlexibleSpace();
					GUILayout.Label($"{level}", style, GUILayout.Width(50), GUILayout.Height(10));
					GUILayout.Label($"{value}", style, GUILayout.Width(50), GUILayout.Height(10));
					GUILayout.FlexibleSpace();
				}, false, 100, 20);
			}

			EditorGUILayout.EndVertical();
		}


		private void Modify() {
			tree.upgradeCost.stats = tree.stats;
			tree.bulletSpeed.stats = tree.stats;
			tree.shootSpeed.stats = tree.stats;
			tree.powerDamage.stats = tree.stats;
			tree.radius.stats = tree.stats;
			EditorUtility.SetDirty(tree);
		}
	}
}