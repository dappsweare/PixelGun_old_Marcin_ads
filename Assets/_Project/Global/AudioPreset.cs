using UnityEngine;
using System.Linq;

namespace Global {
    [System.Serializable]
    public class AudioPreset {

        [Header("Settings")]
        [HideInInspector] public string name = string.Empty;
        [SerializeField] public AudioKey audioKey = AudioKey.None;
        [SerializeField] public AudioClip[] clips = null;

        [Header("Volume")]
        [SerializeField, Range(0, 1)] private float volume = 1;

        [Header("Pitch")]
        [SerializeField, Range(0, 3)] private float pitch = 1;
        [SerializeField] private bool randomPitch = false;
        [SerializeField] private Vector2 pitchRange = Vector2.zero;

        [Header("Info")]
        [HideInInspector] public AudioSource audioSource = null;

        public void OnValidate() => name = audioKey.ToString();

        public void Setup(GameObject audioParent) {
            audioSource = audioParent.AddComponent<AudioSource>();
            audioSource.pitch = pitch;
            audioSource.volume = volume;
        }

        public void Play() => Play(clips.Random());

        public void Play(int clipIndex) => Play(clips[clipIndex]);

        private void Play(AudioClip clip) {
            if (!GameManager.instance.GetAudio()) return;

            float pitchValue = randomPitch ? Random.Range(pitchRange.x, pitchRange.y) : pitch;
            audioSource.pitch = pitchValue;
            audioSource.PlayOneShot(clip);
        }

        public void Stop() => audioSource.Stop();
    }
}