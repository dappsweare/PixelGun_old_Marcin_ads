using Level;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Global {
    public class ScenesManager : Singleton<ScenesManager> {

		[Header("Scenes")]
		[SerializeField] public LevelPreset[] presets = null;

		[Header("Settings")]
		[SerializeField] private bool editorForceLoad = false;

		[Header("Info")]
		[SerializeField, MyReadOnly] public LevelPreset currentScene = null;
		[SerializeField, MyReadOnly] public LevelPreset previousScene = null;

		private Dictionary<string, LevelPreset> presetsByName = null;

		private const string SAVE_CURRENT_LEVEL = "LEVEL_CURRENT";
		private const string SAVE_LEVEL_INDEX = "LEVEL_INDEX";

		private protected override void Awake() {
			base.Awake();
			SetupPresets();
			LoadSave(true);
		}

		private void SetupPresets() {
			presetsByName = new Dictionary<string, LevelPreset>();
			for (int a = 0; a < presets.Length; a++) {
				presetsByName.Add(presets[a].sceneName, presets[a]);
			}
		}

		public void LoadSave(bool force) {
			LevelPreset presetToLoad = PlayerPrefs.HasKey(SAVE_CURRENT_LEVEL) ?
				FindScene(PlayerPrefs.GetString(SAVE_CURRENT_LEVEL)) :
				presets[0];

#if UNITY_EDITOR
			if (editorForceLoad) {
				LoadScene(presetToLoad, force);
			} else {
				currentScene = presetToLoad;
			}
#else
            LoadScene(presetToLoad, force);
#endif
		}

		#region Level Index
		private void SetScene(LevelPreset scenePreset) => PlayerPrefs.SetString(SAVE_CURRENT_LEVEL, scenePreset.sceneName);

		public int GetLevelIndex() => PlayerPrefs.GetInt(SAVE_LEVEL_INDEX, 1);

		private void SetLevelIndex() => PlayerPrefs.SetInt(SAVE_LEVEL_INDEX, GetLevelIndex() + 1);

		#endregion

		private void LoadScene(LevelPreset asset, bool forceLoad) {
			currentScene = asset;
			if (Equals(currentScene.sceneName, CurrentActiveScene())) {
				if (forceLoad)
					SceneManager.LoadScene(asset.sceneName);
			} else {
				SceneManager.LoadScene(asset.sceneName);
			}
		}

		public void NextStage() {
			previousScene = currentScene;
			var allScenes = presets;
			var newScene = allScenes[UnityExtension.Repeat(System.Array.IndexOf(allScenes, currentScene) + 1, allScenes.Length)];

			SetScene(newScene);
			SetLevelIndex();

			ClearData();

			LoadScene(newScene, true);
		}

		public void ResetScene() {
			previousScene = currentScene;
			SetScene(currentScene);

			ClearData();

			LoadScene(currentScene, true);
		}

		public void ClearData() {
			//Players.PlayerUpgrades.instance.ResetUpgrades();
			//Players.PlayerWallet.instance.ResetMoney();
		}


		private string CurrentActiveScene() => currentScene.sceneName;

		private LevelPreset FindScene(string sceneName) => presetsByName[sceneName];

	}
}