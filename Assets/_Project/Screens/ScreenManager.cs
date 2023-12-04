using System.Collections.Generic;
using UnityEngine;

namespace UI {

    public enum ScreenType {
        None,
        Gameplay,
        Upgrade,
        NewGun,
		Victory
	}

	public class ScreenManager : Singleton<ScreenManager> {

        [Header("Screens")]
        [SerializeField] private ScreenBase[] screens = null;

        [Header("Settings")]
        [SerializeField] private ScreenType initScreen = ScreenType.None;

        [Header("Info")]
        [SerializeField, MyReadOnly] public ScreenType currentScreen = ScreenType.None;
        [SerializeField, MyReadOnly] public ScreenType previousScreen = ScreenType.None;
        private Dictionary<ScreenType, ScreenBase> screensPerType = new Dictionary<ScreenType, ScreenBase>();

        private protected override void Awake() {
            base.Awake();
            FillDictionary();
        }

        private void Start() {
            InitScreens(initScreen);
        }

        private void FillDictionary() {
            screensPerType = new Dictionary<ScreenType, ScreenBase>();
            for (int a = 0; a < screens.Length; a++) {
                screensPerType.Add(screens[a].GetScreenType(), screens[a]);
                screens[a].Setup(this);
            }
        }

        private void InitScreens(ScreenType initScreen) {
            if (!screensPerType.ContainsKey(initScreen)) return;

            screensPerType[initScreen].Open();
            currentScreen = initScreen;
        }

        public void ChangeScreen(ScreenType screenTarget) {
            if (currentScreen != ScreenType.None)
                screensPerType[currentScreen].Close();

            previousScreen = currentScreen;
            currentScreen = screenTarget;

            if (currentScreen != ScreenType.None)
                screensPerType[currentScreen].Open();
        }

        public void ChangeScreen(ScreenType screenTarget, float delay) {
            StartCoroutine(Invoker.WaitAndInvoke(() => ChangeScreen(screenTarget), delay));
        }

        public T GetScreen<T>(ScreenType screenTarget) where T : ScreenBase {
            if (screensPerType.ContainsKey(screenTarget)) return screensPerType[screenTarget] as T;
            return null;
        }
    }
}