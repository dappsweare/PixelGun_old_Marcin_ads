using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace UI {
    public class VictoryScreen : ScreenBase {

		[Header("Components")]
		[SerializeField] private RectTransform stageCompleteText = null;
		[SerializeField] private Button nextStageButton = null;

		public override ScreenType GetScreenType() => ScreenType.Victory;

		private protected override void SetupScreen() {
			nextStageButton.onClick.AddListener(OnNextStage);
		}

		private protected override void OnOpen() {
			stageCompleteText.DOAnchorPosY(750, 0f);
			stageCompleteText.DOAnchorPosY(-128, 1f).SetEase(Ease.InOutQuint);
		}

		private void OnNextStage() => Gameplay.GameplayManager.instance.NextStage();
	}
}