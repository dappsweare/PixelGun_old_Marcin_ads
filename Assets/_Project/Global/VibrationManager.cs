using UnityEngine;
using Lofelt.NiceVibrations;

namespace Global {
    public class VibrationManager : Singleton<VibrationManager> {

        [Header("Settings")]
        private float vibrationDelay = 1.5f;
        private bool vibrationAvailable = true;


        public void Vibrate() {
            if (!GameManager.instance.GetHaptics() || !vibrationAvailable) return;

            PlayVibration();

            StartCoroutine(Invoker.WaitAndInvoke(() => {
                vibrationAvailable = true;
            }, vibrationDelay));
        }

        private void PlayVibration() {
#if UNITY_EDITOR
            //Debug.Log("Wzzz");
#elif UNITY_IOS
		HapticPatterns.PlayPreset(HapticPatterns.PresetType.MediumImpact);	
#elif UNITY_ANDROID
		HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
#endif

        }
    }
}