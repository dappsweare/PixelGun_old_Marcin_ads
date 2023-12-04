using System.Collections.Generic;
using UnityEngine;


namespace Level {
	[CreateAssetMenu(fileName = "Level Preset - ", menuName = "SO/Level/Preset", order = 0)]
	public class LevelPreset : ScriptableObject {

		[Header("Scenes")]
		public string sceneName = string.Empty;

		[Header("Components")]
		public Texture2D texture = null;

		[Header("Settings")]
		public List<LevelColors> levelColors = new List<LevelColors>();
		public int pixelSize = 1;
		public float pixelScale = 1f;
		public float worldScale = 1f;

		[Header("Health")]
		public float minHealth = 1f;
		public float maxHealth = 100f;

		public int Width { get => texture.width; }
		public int Height { get => texture.height; }

		public bool IsEmpty(int x, int y) => Equals(Color.clear, texture.GetPixel(x, y));

		public bool IsEmptyUnder(int x, int y) {
			if (x < 0 || x >= Width || y < 0 || y >= Height || y - 1 < 0 || y - 1 >= Height) return false;
			return IsEmpty(x, y - 1);
		}

		public Color GetColor(int x, int y) {
			LevelColors levelColor = GetLevelColors(x, y);
			if (levelColor != null)
				return levelColor.targetColor;
			return Color.white;
		}

		public LevelColors GetLevelColors(int x, int y) {
			Color pixelColor = texture.GetPixel(x, y);
			for (int a = 0; a < levelColors.Count; a++) {
				if (Equals(levelColors[a].pixelColor, pixelColor)) {
					return levelColors[a];
				}
			}

			Debug.LogError("Lack of compliance!");
			return null;
		}

		public int GetLevelColorsIndex(int x, int y) {
			LevelColors levelColor = GetLevelColors(x, y);
			if (levelColor != null)
				return levelColors.IndexOf(levelColor);
			return 0;
		}

		public Vector3 GetWorldCenter() {
			float finalWidth = Width * pixelScale * pixelSize;
			float finalHeight = Height * pixelScale * pixelSize;
			float centerX = -finalWidth / 2 + pixelScale / 2;
			float centerY = -finalHeight / 2 + pixelScale / 2;
			return new Vector3(centerX, centerY, 0f);
		}
	}
}