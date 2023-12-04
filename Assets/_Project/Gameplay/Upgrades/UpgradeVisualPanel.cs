using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Upgrades {
	public class UpgradeVisualPanel : MonoBehaviour {

		[Serializable]
		private protected class ColorScheme {
			public Color outline;
			public Color background;
			public Color foreground;
			public Color text;
		}

		[Header("Components")]
		[SerializeField] private protected TMP_Text titleText = null;
		[SerializeField] private protected TMP_Text levelText = null;
		[SerializeField] private protected TMP_Text costText = null;

		[Header("Images")]
		[SerializeField] private Image outline = null;
		[SerializeField] private Image background = null;
		[SerializeField] private Image foreground = null;

		[Header("Color Scheme")]
		[SerializeField] private protected ColorScheme activeScheme = null;
		[SerializeField] private protected ColorScheme inactiveScheme = null;
		[SerializeField] private protected ColorScheme maxScheme = null;

		public virtual void Refresh(UpgradePreset preset) {
			string displayName = preset.panelDisplayName;
			int level = preset.GetLevel();
			float cost = preset.NextLevelCost();
			bool isMaxLevel = IsMaxLevel(preset);

			titleText.text = string.Format("<b>{0}</b>", displayName);
			levelText.text = !isMaxLevel ? string.Format("Lvl. {0}", level) : string.Empty;
			costText.text = !isMaxLevel ? string.Format("<sprite=2> ${0}", cost) : "MAX";

			ColorScheme colorScheme = isMaxLevel ? maxScheme : !preset.CanAfford(Players.PlayerWallet.instance.GetMoney()) ? inactiveScheme : activeScheme;
			ApplyScheme(colorScheme);
		}

		private protected void ApplyScheme(ColorScheme colorScheme) {
			outline.color = colorScheme.outline;
			background.color = colorScheme.background;
			foreground.color = colorScheme.foreground;
			titleText.color = colorScheme.text;
			levelText.color = colorScheme.text;
			costText.color = colorScheme.text;
		}

		private protected virtual bool IsMaxLevel(UpgradePreset preset) => preset.IsMaxLevel();

		public virtual bool CanUpgrade(UpgradePreset preset) => preset.CanUpgrade();

		public virtual void OnUpgrade(UpgradePreset preset) { }
	}
}