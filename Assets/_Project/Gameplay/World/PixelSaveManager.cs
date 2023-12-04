using Level;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameplay {
    public class PixelSaveManager : Singleton<PixelSaveManager> {

		private const string SAVE_NAME = "LEVEL_PROGRESS";


		private void Update() {
			if (Input.GetKeyDown(KeyCode.F5)) {
				SaveScene();
			}

			if (Input.GetKeyDown(KeyCode.F6)) {
				LoadScene();
			}
		}

		private void OnApplicationPause(bool pauseStatus) {
			if (pauseStatus)
				SaveScene();
		}

		public void LoadScene() {
			Debug.Log(" - Load Scene - ");

			string save = PlayerPrefs.GetString(SAVE_NAME);
			if (string.IsNullOrEmpty(save)) return;
			List<string> pixelsData = save.Split('[', ']').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
			for (int a = 0; a < pixelsData.Count; a++) {
				List<string> pixelInfo = pixelsData[a].Split('-').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
				int index = int.Parse(pixelInfo[0]);
				int exist = int.Parse(pixelInfo[1]);

				if (exist == 0)
					PixelController.instance.RemovePixel(index, false);
			}
		}

		public void SaveScene() {
			Debug.Log(" - Save Scene - ");
			string savefile = string.Empty;

			int totalWidth = PixelController.instance.consturctor.totalWidth;
			int totalHeight = PixelController.instance.consturctor.totalHeight;
			LevelPixel[] totalPixels = PixelController.instance.consturctor.totalPixels;

			for (int x = 0; x < totalWidth; x++) {
				for (int y = 0; y < totalHeight; y++) {
					int index = x + (y * totalWidth);
					bool exist = totalPixels[index] != null;
					savefile += string.Format("[{0}-{1}]", index, exist ? 1 : 0);
				}
			}

			PlayerPrefs.SetString(SAVE_NAME, savefile);
		}

		public void ClearSave() => PlayerPrefs.DeleteKey(SAVE_NAME);

		public bool ContainSave() => PlayerPrefs.HasKey(SAVE_NAME);
	}
}