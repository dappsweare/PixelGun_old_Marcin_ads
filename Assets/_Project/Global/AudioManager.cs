using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Global {
    public class AudioManager : Singleton<AudioManager> {

        [Header("Components")]
        [SerializeField] private AudioPreset[] presets = null;

        private Dictionary<AudioKey, AudioPreset> audioPresets = null;

        private void OnValidate() {
            presets.ToList().ForEach((x) => x.OnValidate());
        }

        private protected override void Awake() {
            base.Awake();
            audioPresets = new Dictionary<AudioKey, AudioPreset>();
            for (int a = 0; a < presets.Length; a++) {
                audioPresets.Add(presets[a].audioKey, presets[a]);
                presets[a].Setup(gameObject);
            }
        }

        public void Play(AudioKey key) => audioPresets[key].Play();
        public void Play(AudioKey key, int index) => audioPresets[key].Play(index);
        public void Stop(AudioKey key) => audioPresets[key].Stop();
    }

    public enum AudioKey {
        None = 0,

        // UI
        [InspectorName("UI - Button")] UI_Button = 1,

        // Guns
        [InspectorName("Gun - Pistol")] GUN_Pistol = 2,
        [InspectorName("Gun - Revolver")] GUN_Revolver = 3,
        [InspectorName("Gun - Uzi")] GUN_Uzi = 4,
        [InspectorName("Gun - Submachine")] GUN_Submachine = 5,
        [InspectorName("Gun - Assault Rifle")] GUN_AssaultRifle = 6,
        [InspectorName("Gun - Crossbow")] GUN_Crossbow = 7,
        [InspectorName("Gun - Throw")] GUN_Throw = 8,
	}
}