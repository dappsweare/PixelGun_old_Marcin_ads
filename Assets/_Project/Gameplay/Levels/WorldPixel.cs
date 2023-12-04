using UnityEngine;

namespace Level {
    [SelectionBase]
    public class WorldPixel : MonoBehaviour {

        [Header("Components")]
        [SerializeField] private MeshRenderer meshRenderer = null;

        public void Initalize(Color targetColor, Vector3 scale) {
            transform.localScale = scale;
            transform.eulerAngles = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));

            meshRenderer.ModifyColor(targetColor);
        }
    }
}