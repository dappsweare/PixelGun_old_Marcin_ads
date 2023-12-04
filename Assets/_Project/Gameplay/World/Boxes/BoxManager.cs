using UnityEngine;

namespace World {
	public class BoxManager : Singleton<BoxManager> {

		[Header("Components")]
		[SerializeField] private BoxCoin coinPrefab = null;

		[Header("Info")]
		private Pool.Pool<BoxCoin> coinPool = null;

		private void Start() {
			coinPool = new Pool.Pool<BoxCoin>(coinPrefab, transform, 5);
		}

		public void CreateCoin(Transform box, float value, Color color, Vector3 velocity, Vector3 scale) {
			var coin = coinPool.Allocate();
			Vector3 spawnPoint = box.transform.position;
			spawnPoint.z += Random.Range(-.5f, .5f);
			coin.transform.position = spawnPoint;
			coin.gameObject.SetActive(true);
			coin.Initialize(value, color, velocity, scale, coinPool);
		}
	}
}