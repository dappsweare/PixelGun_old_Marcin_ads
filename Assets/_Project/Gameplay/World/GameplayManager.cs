using Level;
using UnityEngine;
using DG.Tweening;
using UI;

namespace Gameplay {
	public class GameplayManager : Singleton<GameplayManager> {

		[Header("Prefabs")]
		[SerializeField] private PixelController pixelControllerPrefab = null;

		[Header("Info")]
		private bool victory = false;

		private void Start() => GameStarted();

		public void GameStarted() {
			if (PixelSaveManager.instance.ContainSave()) {
				PixelSaveManager.instance.LoadScene();
			}
		}

		public void Victory(float delay) {
			if (ScreenManager.instance.currentScreen != ScreenType.Gameplay) return;

			if (victory) return;
			victory = true;
			PixelSaveManager.instance.ClearSave();
			StartCoroutine(Invoker.WaitAndInvoke(() => {
				UI.ScreenManager.instance.ChangeScreen(UI.ScreenType.Victory);
				Destroy(PixelController.instance.gameObject);
				GameObjects.GOInstantiate(pixelControllerPrefab);
			}, delay));
		}

		public void NextStage() {
			DOTween.Clear(true);
			Global.ScenesManager.instance.NextStage();
			UI.ScreenManager.instance.ChangeScreen(UI.ScreenType.Gameplay);
			var gameplayScreen = UI.ScreenManager.instance.GetScreen<GameplayScreen>(ScreenType.Gameplay);
			gameplayScreen.RefreshUpgrades();
		}
	}
}