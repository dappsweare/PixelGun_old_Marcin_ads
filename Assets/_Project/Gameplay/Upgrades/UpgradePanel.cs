using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Upgrades {
	public class UpgradePanel : MonoBehaviour {

		[Header("Components")]
		[SerializeField] private UpgradeVisualPanel visual = null;
		[SerializeField] private UpgradePreset preset = null;
		[SerializeField] private Button button = null;

		[Header("Info")]
		[SerializeField] private bool upgradePreset = true;

		private void OnEnable() {
			button.onClick.AddListener(OnClick);
			Refresh();
		}

		private void OnDisable() {
			button.onClick.RemoveListener(OnClick);
		}

		private void Start() => Refresh();

		public void Refresh() => visual.Refresh(preset);

		private void OnClick() {
			if (!CanUpgrade()) return;

			OnUpgrade();
			if (upgradePreset) {
				preset.LevelUp();
				Players.PlayerWallet.instance.RemoveMoney(preset.Cost());
			}

			transform.DORestart();
			transform.localScale = Vector3.one * 1.25f;
			transform.DOScale(Vector3.one, .5f).SetEase(Ease.InOutCirc);

			Global.AudioManager.instance.Play(Global.AudioKey.UI_Button);
		}

		private bool CanUpgrade() => visual.CanUpgrade(preset);

		private void OnUpgrade() => visual.OnUpgrade(preset);
	}
}