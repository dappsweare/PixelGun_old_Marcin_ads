using UnityEngine;
namespace UI {
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class ScreenBase : MonoBehaviour {

        [Header("Info")]
        [SerializeField, MyReadOnly] private protected RectTransform rectTransform = null;
        [SerializeField, MyReadOnly] private protected CanvasGroup canvasGroup = null;
        private ScreenManager screenManager = null;

        private void OnEnable() => OnEnableAction();

        private void OnDisable() => OnDisableAction();

        public void Setup(ScreenManager screenManager) {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            rectTransform.anchoredPosition = Vector2.zero;
            this.screenManager = screenManager;
            SetupScreen();
            SetScreen(false);
        }

        public void Open() {
            SetScreen(true);
            OnOpen();
        }

        public void Close() {
            SetScreen(false);
            OnClose();
        }

        private void SetScreen(bool value) {
            SetAlpha(value ? 1f : 0f);
            canvasGroup.interactable = value;
            canvasGroup.blocksRaycasts = value;
        }

        public void SetAlpha(float value) {
            canvasGroup.alpha = value;
        }


        public abstract ScreenType GetScreenType();


        private protected virtual void OnEnableAction() { }

        private protected virtual void OnDisableAction() { }


        private protected virtual void SetupScreen() { }


        private protected virtual void OnOpen() { }

        private protected virtual void OnClose() { }


    }
}