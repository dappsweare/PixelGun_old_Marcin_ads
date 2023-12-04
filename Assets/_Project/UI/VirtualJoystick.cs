using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class VirtualJoystick : Singleton<VirtualJoystick>, IPointerDownHandler, IDragHandler, IPointerUpHandler {

	[Header("Components")]
	[SerializeField] private CanvasGroup canvasGroup = null;

	[Header("Rect References")]
	[SerializeField] private RectTransform containerRect;
	[SerializeField] private RectTransform handleRect;

	[Header("Settings")]
	[SerializeField] private float joystickRange = 50f;

	private void OnEnable() {
		Players.PlayerInput.OnTripleTap += OnTripleTap;
	}

	private void OnDisable() {
		Players.PlayerInput.OnTripleTap -= OnTripleTap;
	}

	private void Start() {
		canvasGroup.alpha = 0f;
		UpdateHandleRectPosition(Vector2.zero);
	}

	private void OnTripleTap() {
#if !UNITY_EDITOR
		ForceHide();
		return;
#endif
	}


	public void OnPointerDown(PointerEventData eventData) {
#if !UNITY_EDITOR
		if (Input.touchCount > 1) {
			ForceHide();
			return;
		}
#endif
		Global.GameManager.instance.joystickController = true;
		containerRect.position = eventData.position;
		handleRect.anchoredPosition = Vector2.zero;
		OnDrag(eventData);
		StartFade(1f, .5f, 0f);
	}

	public void OnDrag(PointerEventData eventData) {
#if !UNITY_EDITOR
		if (Input.touchCount > 1) {
			ForceHide();
			return;
		}
#endif

		RectTransformUtility.ScreenPointToLocalPointInRectangle(containerRect, eventData.position, eventData.pressEventCamera, out Vector2 position);
		position = ApplySizeDelta(position);
		Vector2 clampedPosition = ClampValuesToMagnitude(position);
		OutputPointerEventValue(clampedPosition);
		UpdateHandleRectPosition(clampedPosition * joystickRange);
	}

	public void OnPointerUp(PointerEventData eventData) {
#if !UNITY_EDITOR
		if (Input.touchCount > 1) {
			ForceHide();
			return;
		}
#endif

		Global.GameManager.instance.joystickController = false;

		OutputPointerEventValue(Vector2.zero);
		UpdateHandleRectPosition(Vector2.zero);
		StartFade(0f, .5f, .75f);
	}

	private void OutputPointerEventValue(Vector2 pointerPosition) {
		Weapons.WeaponController.instance.JoystickPosition = pointerPosition;
	}

	private void UpdateHandleRectPosition(Vector2 newPosition) {
		if (handleRect)
			handleRect.anchoredPosition = newPosition;
	}

	private Vector2 ApplySizeDelta(Vector2 position) {
		float x = (position.x / containerRect.sizeDelta.x) * 2.5f;
		float y = (position.y / containerRect.sizeDelta.y) * 2.5f;
		return new Vector2(x, y);
	}

	private Vector2 ClampValuesToMagnitude(Vector2 position) => Vector2.ClampMagnitude(position, 1);

	private void ForceHide() {
		canvasGroup.alpha = 0;
	}

	private void StartFade(float targetAlpha, float duration, float delay) {
		canvasGroup.DOKill();
		canvasGroup.DOFade(targetAlpha, duration).SetDelay(delay);
	}
}