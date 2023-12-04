using UnityEngine;
using DG.Tweening;

namespace Global {
    public class GameManager : Singleton<GameManager> {

        [Header("Settings")]
        public static string SAVE_HAPTICS = "SAVE_HAPTICS";
        public static string SAVE_AUDIO = "SAVE_AUDIO";

        [Header("Game Settings")]
        [SerializeField] public int startMoneyAmount = 100;
        [SerializeField] public bool joystickController = false;

        private protected override void Awake() {
            base.Awake();
            Application.targetFrameRate = 60;

            InitializeTweener();

            SetAudio(true);
            SetHaptics(true);
        }

        public void SetHaptics(bool value) => PlayerPrefs.SetInt(SAVE_HAPTICS, value ? 1 : 0);

        public bool GetHaptics() => PlayerPrefs.GetInt(SAVE_HAPTICS, 1) == 1;


        public void SetAudio(bool value) => PlayerPrefs.SetInt(SAVE_AUDIO, value ? 1 : 0);

        public bool GetAudio() => PlayerPrefs.GetInt(SAVE_AUDIO, 1) == 1;

        private void InitializeTweener() {
            bool autoKillMode = false;
            bool useSafeMode = false;
            LogBehaviour logBehaviour = LogBehaviour.Default;
            int tweenersCapacity = 500;
            int sequencesCapacity = 50;

            DOTween.Init(autoKillMode, useSafeMode, logBehaviour)
                .SetCapacity(tweenersCapacity, sequencesCapacity);
        }

    }
}