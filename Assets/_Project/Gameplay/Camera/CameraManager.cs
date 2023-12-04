using UnityEngine;

namespace Gameplay {
    public class CameraManager : Singleton<CameraManager> {

        [Header("Components")]
        [SerializeField] public Camera weaponCamera = null;

		private void Start() {
            Vector3 currentPosition = transform.position;
            currentPosition.z = -(32f * 9f / 16f);
            transform.position = currentPosition;
        }
    }
}