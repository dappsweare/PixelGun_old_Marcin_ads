using UnityEngine;

namespace Level {
	public class PixelConsturctor : MonoBehaviour {

		[Header("Components")]
		[SerializeField] private PixelController pixelController = null;

		[Header("Prefabs")]
		[SerializeField] public LevelPixel pixelPrefab = null;
		[SerializeField] public Transform pixelParent = null;
		[SerializeField] public Material pixelMaterialPreset = null;

		[Header("Info")]
		private LevelPreset currentPreset = null;

		[Header("Save")]
		[MyReadOnly] public int totalWidth;
		[MyReadOnly] public int totalHeight;
		[MyReadOnly] public LevelPixel[] totalPixels = null;


		public void CreateLevel(LevelPreset currentPreset) {
			this.currentPreset = currentPreset;

			int pixelSize = currentPreset.pixelSize;
			int height = currentPreset.Height;
			int width = currentPreset.Width;

			for (int a = 0; a < currentPreset.levelColors.Count; a++) {
				Material newMaterial = new Material(pixelMaterialPreset) {
					color = currentPreset.levelColors[a].targetColor
				};
				pixelController.pixelMaterials.Add(newMaterial);
			}

			pixelParent.localPosition = Vector3.zero;

			totalWidth = width * pixelSize;
			totalHeight = height * pixelSize;
			totalPixels = new LevelPixel[totalWidth * totalHeight];

			for (int y = 0; y < height; y++) {
				for (int x = 0; x < width; x++) {
					if (currentPreset.IsEmpty(x, y)) continue;
					CreatePixels(x, y);
				}
			}
		}

		public void ClearLevel() {
			pixelParent.DestroyAllChildren();
		}


		public LevelPixel GetPixel(int x, int y) {
			if (x < 0 || x >= totalWidth || y < 0 || y >= totalHeight) return null;

			int index = x + (y * totalWidth);
			return index < 0 || index >= totalPixels.Length ? null : totalPixels[index];
		}


		private void CreatePixels(int textureX, int textureY) {
			int pixelSize = currentPreset.pixelSize;
			int totalHeight = currentPreset.Height * currentPreset.pixelSize;

			for (int y = 0; y < pixelSize; y++) {
				for (int x = 0; x < pixelSize; x++) {
					int finalX = x + (textureX * pixelSize);
					int finalY = y + (textureY * pixelSize);
					LevelPixel pixel = CreatePixel(finalX, finalY);

					Material material = pixelController.pixelMaterials[currentPreset.GetLevelColorsIndex(textureX, textureY)];
					Color targetColor = currentPreset.GetColor(textureX, textureY);
					float healthTime = (float)finalY / totalHeight;

					Coordinates coordinates = new Coordinates(finalX, finalY, totalWidth);
					pixel.Initalize(currentPreset, material, targetColor, healthTime, coordinates);
					pixelController.pixels.Add(pixel);
					totalPixels[coordinates.Index] = pixel;
				}
			}

		}

		private LevelPixel CreatePixel(int finalX, int finalY) {
#if UNITY_EDITOR
			var pixel = UnityEditor.PrefabUtility.InstantiatePrefab(pixelPrefab) as LevelPixel;
			pixel.transform.position = GetCellPosition(finalX, finalY) + currentPreset.GetWorldCenter();
			pixel.transform.eulerAngles = Vector3.zero;
			pixel.transform.localScale = GetScale();
			pixel.transform.SetParent(pixelParent);
			return pixel;
#else
			return null;
#endif

			Vector3 GetCellPosition(int x, int y) {
				float xPos = x * currentPreset.pixelScale;
				float yPos = y * currentPreset.pixelScale;
				float zPos = Random.Range(-.15f, .15f);
				return new Vector3(xPos, yPos, zPos);
			}

			Vector3 GetScale() => Vector3.one * currentPreset.pixelScale;
		}
	}
}