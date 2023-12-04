using UnityEngine;
using TMPro;
using DG.Tweening;

namespace UI {
    public class FloatingText : MonoBehaviour {

        [Header("Components")]
        [SerializeField] private RectTransform rectTransform = null;
        [SerializeField] public TMP_Text floatingText = null;

        [Header("Settings")]
        [SerializeField] private float duration = 1f;

        public void Initialize(string text, System.Action onComplete) {
            floatingText.alpha = 1;
			floatingText.text = text;

            float currentY = rectTransform.position.y;
            rectTransform.DOMoveY(currentY + 250, duration);
            floatingText.DOFade(0, duration).OnComplete(() => { onComplete.Invoke(); });
		}
    }
}