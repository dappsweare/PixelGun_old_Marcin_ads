using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Weapons;

namespace UI {
    public class NewGunUnlockedScreen : ScreenBase {

		[Header("Components")]
		[SerializeField] private Image backgroundImage = null;
		[SerializeField] private RectTransform backgroundTransform = null;
		[SerializeField] private Button contiuneButton = null;

		[Header("Settings")]

		[SerializeField] private RectTransform[] rays = null;
		[SerializeField] private float[] raysSpeed = null;

		public override ScreenType GetScreenType() => ScreenType.NewGun;

		private protected override void SetupScreen() {
			contiuneButton.onClick.AddListener(OnContiuneButton);
		}

		private protected override void OnOpen() {
			backgroundImage.color = Color.clear;
			backgroundImage.DOColor(new Color(0, 0, 0, .5f), .5f);

			backgroundTransform.DOAnchorPosY(1600, 0f);
			backgroundTransform.DOAnchorPosY(0, 1f).SetEase(Ease.InOutQuint);
		}

		private void Update() {
			for (int a = 0; a < rays.Length; a++) {
				rays[a].eulerAngles += raysSpeed[a] * Time.deltaTime * Vector3.forward;
			}
		}

		private void OnContiuneButton() {
			WeaponController.instance.CloseNewWeapon();
		}
	}
}