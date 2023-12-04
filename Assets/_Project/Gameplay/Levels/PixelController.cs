using System.Collections.Generic;
using UnityEngine;

namespace Level {
	public class PixelController : Singleton<PixelController> {

		[Header("Components")]
		[SerializeField] public PixelConsturctor consturctor = null;

		[Header("Settings")]
		[SerializeField] public LevelPreset currentPreset = null;
		[SerializeField] public WorldPixel worldPixelPrefab = null;
		[SerializeField] public Transform worldPixelParent = null;
		[SerializeField] public List<Material> pixelMaterials = null;

		[Header("Particles")]
		[SerializeField] private ParticleSystem pixelHit = null;

		[Header("Info")]
		[SerializeField, MyReadOnly] public List<LevelPixel> pixels = null;
		private Pool.Pool<ParticleSystem> hitPool = null;

		private void Start() {
			hitPool = new Pool.Pool<ParticleSystem>(pixelHit, transform, 10);
		}


		public void CreateLevel() {
			if (pixels == null) pixels = new List<LevelPixel>();
			if (pixelMaterials == null) pixelMaterials = new List<Material>();
			consturctor.CreateLevel(currentPreset);
		}

		public void ClearLevel() {
			if (pixels != null) pixels.Clear();
			if (pixelMaterials != null) pixelMaterials.Clear();
			consturctor.ClearLevel();
		}

		
		public LevelPixel GetClosestPixel(Transform point) {
			var levelPixel = TransformExtension.GetClosestObject<LevelPixel>(point.position, pixels);
			return levelPixel;
		}

		public List<LevelPixel> GetPixels(Transform point, float radius) {
			List<LevelPixel> levelPixels = new List<LevelPixel>();
			for (int a = 0; a < pixels.Count; a++) {
				if (Vector3.Distance(point.position, pixels[a].transform.position) <= radius) {
					levelPixels.Add(pixels[a]);
				}
			}
			return levelPixels;

		}

		
		public bool ContainsPixels() => pixels.Count > 0;

		public void LevelPixelHit(LevelPixel pixel) {
			EmitParticles(hitPool, pixel.transform.position, pixel.TargetColor());
		}

		public void RemovePixel(LevelPixel pixel, bool createWorldPixel) {
			if (createWorldPixel) {
				CreateWorldPixel(pixel);
			}
			pixels.Remove(pixel);
			Destroy(pixel.gameObject);

			if (pixels.Count <= 0) {
				Gameplay.GameplayManager.instance.Victory(.5f);
			}
		}

		public void RemovePixel(int index, bool createWorldPixel) {
			var pixel = consturctor.totalPixels[index];
			if (pixel != null)
				RemovePixel(pixel, createWorldPixel);
		}

		private void CreateWorldPixel(LevelPixel pixel, int amount = 1) {
			var targetColor = pixel.TargetColor();
			var scale = Vector3.one * currentPreset.worldScale;
			Vector3 spawnPosition = new Vector3(pixel.transform.position.x, pixel.transform.position.y, 3f);
			EmitParticles(hitPool, pixel.transform.position, pixel.TargetColor());
			for (int a = 0; a < amount; a++) {
				var worldPixel = GameObjects.GOInstantiate(worldPixelPrefab, "World Pixel", spawnPosition, Vector3.zero, worldPixelParent);
				worldPixel.Initalize(targetColor, scale);
			}
		}

		private void EmitParticles(Pool.Pool<ParticleSystem> pool, Vector3 position, Color color) {
			ParticleSystem particle = pool.Allocate();
			var main = particle.main;
			main.startColor = color.ModifyColor(Random.Range(-.2f, .2f));
			particle.gameObject.SetActive(true);
			particle.transform.position = position;

			particle.Play();
			if (particle.TryGetComponent(out ParticleController particleController)) {
				particleController.OnParticleComplete += () => OnParticleComplete(particleController, particle);
			}

			void OnParticleComplete(ParticleController particleController, ParticleSystem particle) {
				particleController.ClearActions();
				particle.gameObject.SetActive(false);
				pool.Release(particle);
			}
		}
	}
}