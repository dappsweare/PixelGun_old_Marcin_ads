using UnityEngine;

namespace Players {
    public class PlayerWallet : Singleton<PlayerWallet> {

        private const string MONEY_SAVE_NAME = "PLAYER_COIN";
		public static System.Action OnMoneyModify;
		public static System.Action<float> OnSetMoney;

		private void Start() {
			OnSetMoney?.Invoke(GetMoney());
			OnMoneyModify.Invoke();
		}

		public void AddMoney(float value) => ModifyMoney(Mathf.Abs(value));

		public void RemoveMoney(float value) => ModifyMoney(-Mathf.Abs(value));

		public void ModifyMoney(float value) {
			float save = GetMoney();
			save = Mathf.Clamp(save + value, 0, Mathf.Infinity);
			save = (float)System.Math.Round(save, 2);

			SetMoney(save);
			OnSetMoney?.Invoke(save);
			OnMoneyModify?.Invoke();
		}


		public float GetMoney() => PlayerPrefs.GetFloat(MONEY_SAVE_NAME, Global.GameManager.instance.startMoneyAmount);

		private void SetMoney(float value) => PlayerPrefs.SetFloat(MONEY_SAVE_NAME, value);

		public void ResetMoney() {
			PlayerPrefs.DeleteKey(MONEY_SAVE_NAME);
		}
	}
}