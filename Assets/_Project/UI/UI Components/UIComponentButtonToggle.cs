using UnityEngine;
using UnityEngine.UI;

namespace UI {
    [RequireComponent(typeof(Button))]
    public abstract class UIComponentButtonToggle : MonoBehaviour {

        [Header("Components")]
        [SerializeField] private Button button = null;
        [SerializeField] private Image icon = null;

		[Header("Settings")]
        [SerializeField] private Sprite onSprite = null;
        [SerializeField] private Sprite offSprite = null;

        private void Start() {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
        }

        private protected void Toggle(bool isOn) => icon.sprite = isOn ? onSprite : offSprite;

        public abstract void OnClick();
    }
}