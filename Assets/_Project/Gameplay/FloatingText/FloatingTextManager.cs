using UnityEngine;

namespace UI {
    public class FloatingTextManager : Singleton<FloatingTextManager> {

        [Header("Prefabs")]
        [SerializeField] private FloatingText floatingTextPrefab = null;

        [Header("Info")]
        private Pool.Pool<FloatingText> floatingPool = null;

        private protected override void Awake() {
            base.Awake();
            floatingPool = new Pool.Pool<FloatingText>(floatingTextPrefab, transform, 10);
        }


        public void CreateText(Vector3 position, string text, Color color) {
			FloatingText floatingText = floatingPool.Allocate();
			floatingText.transform.position = Camera.main.WorldToScreenPoint(position);
			floatingText.gameObject.SetActive(true);
            floatingText.floatingText.color = color;

			floatingText.Initialize(text, () => OnComplete(floatingText));
		}

        private void OnComplete(FloatingText floatingText) {
            floatingPool.Release(floatingText);
            floatingText.gameObject.SetActive(true);

		}
    }
}