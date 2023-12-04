using UnityEngine;
//using DG.Tweening;

namespace UI {
	public class UIComponentMoneyText : MonoBehaviour {

		[Header("Components")]
		//[SerializeField] private Transform moneyIcon = null;
		[SerializeField] private TMPro.TMP_Text moneyText = null;

		private void OnEnable() {
			Players.PlayerWallet.OnSetMoney += OnSetMoney;
		}

		private void OnDisable() {
			Players.PlayerWallet.OnSetMoney -= OnSetMoney;
		}

		private void OnSetMoney(float amount) {
			moneyText.text = $"<sprite=2> ${amount}";

			//moneyIcon.localScale = Vector3.one * 1.25f;
			//moneyIcon.DOScale(Vector3.one, .5f).SetEase(Ease.InOutCirc);
		}
	}
}