using UnityEngine;

namespace Level {
	//[SelectionBase]
	public class LevelPixel : MonoBehaviour {

		[Header("Components")]
		[SerializeField] private MeshRenderer[] meshRenderers = null;

		[Header("Health")]
		[SerializeField] private float health = 0f;
		[SerializeField] private float initHealth = 0f;

		[Header("Info")]
		[SerializeField, MyReadOnly] private Color targetColor = Color.white;

		[Header("Save")]
		[MyReadOnly] public Coordinates coordinates = null;


		public void Initalize(LevelPreset levelPreset, Material material, Color targetColor, float healthTime, Coordinates coordinates) {
			this.targetColor = targetColor;
			this.coordinates = coordinates;

			SetMaterials(material);
			initHealth = health = Mathf.Lerp(levelPreset.minHealth, levelPreset.maxHealth, healthTime);
		}

		public void DealDamage(float power) {
			health = Mathf.Clamp(health - power, 0f, initHealth);

			if (health <= 0) {
				Death();
			} else {
				Hit();
			}
		}

		public void Death() => PixelController.instance.RemovePixel(this, true);

		public void Hit() {
			PixelController.instance.LevelPixelHit(this);
			SetColor();
		}

		public Color TargetColor() => targetColor;

		private void SetMaterials(Material material) {
			for (int a = 0; a < meshRenderers.Length; a++) {
				meshRenderers[a].material = material;
			}
		}

		private void SetColor() {
			for (int a = 0; a < meshRenderers.Length; a++) {
				meshRenderers[a].ModifyColor(Color.Lerp(targetColor, Color.black, 1 - (health / initHealth)));
			}
		}
	}


	[System.Serializable]
	public class Coordinates {
		public int x;
		public int y;
		public int totalWidth;

		public int Index {
			get
			{
				return x + (y * totalWidth);
			}
		}

		public Coordinates(int x, int y, int totalWidth) {
			this.x = x;
			this.y = y;
			this.totalWidth = totalWidth;
		}
	}
}