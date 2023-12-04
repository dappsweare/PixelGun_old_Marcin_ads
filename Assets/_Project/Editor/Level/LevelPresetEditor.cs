using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace Level {
	[CanEditMultipleObjects]
	[CustomEditor(typeof(LevelPreset))]
	public class LevelPresetEditor : Editor {

		private List<string> currentScenes = null;

		private void OnEnable() {
			SetupScenes();
		}

		public override void OnInspectorGUI() {
			base.OnInspectorGUI();

			GUILayout.Space(5);

			if (GUILayout.Button("Setup")) SetupPresets();

			var style = Dapps.EditorUtilities.HorizontalLineStyle();
			style.normal.textColor = IsValid() ? Color.green : Color.red;

			GUILayout.Label("VALID", style, GUILayout.ExpandWidth(true));
		}

		private void SetupPresets() {
			for (int a = 0; a < targets.Length; a++) {
				if (targets[a] is LevelPreset levelPreset) {
					SetupPreset(levelPreset);
				}
			}
		}

		private void SetupPreset(LevelPreset levelPreset) {
			if (levelPreset.levelColors != null)
				levelPreset.levelColors.Clear();
			else
				levelPreset.levelColors = new List<LevelColors>();
			for (int y = 0; y < levelPreset.Height; y++) {
				for (int x = 0; x < levelPreset.Width; x++) {
					Color pixelColor = levelPreset.texture.GetPixel(x, y);
					if (Equals(pixelColor, Color.clear)) continue;

					var settings = levelPreset.levelColors.FirstOrDefault(x => x.pixelColor == pixelColor);
					if (settings == null) {
						settings = new LevelColors(pixelColor);
						if (Equals(pixelColor, Color.black)) {
							ColorUtility.TryParseHtmlString("#2F2F2F", out Color targetColor);
							settings.targetColor = targetColor;
						}
						levelPreset.levelColors.Add(settings);
					}
				}

				EditorUtility.SetDirty(levelPreset);
			}
		}
	
		private bool IsValid() {
			for (int i = 0; i < currentScenes.Count; i++) {
				if (Equals(currentScenes[i], ((LevelPreset)target).sceneName))
					return true;
			}

			return false;
		}

		private void SetupScenes() {
			currentScenes = new List<string>();
			string[] guids = AssetDatabase.FindAssets("t:SceneAsset", null);
			for (int i = 0; i < guids.Length; i++) {
				string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
				SceneAsset asset = AssetDatabase.LoadAssetAtPath<SceneAsset>(assetPath);
				if (asset == null) continue;
				string sceneName = asset.name;
				currentScenes.Add(sceneName);
			}
		}
	}
}
