using UnityEngine;

namespace Upgrades {
	[CreateAssetMenu(fileName = "Upgrade Preset", menuName = "SO/Upgrades/Upgrade Preset", order = 0)]
	public class UpgradePreset : ScriptableObject {

		public enum UpgradeType {
			None,
			Speed,
			Power,
			Income,
			NewGun
		}

		[System.Serializable]
		public class Multiplier {
			[SerializeField] private protected float initValue;
			[SerializeField] private protected float multiplierValue = 1f;
			[SerializeField] private protected int decimalPlaces = 2;

			public virtual float GetValue(int level) {
				var value = initValue * level * multiplierValue;
				var roundedValue = System.Math.Round(value, decimalPlaces);
				return (float)roundedValue;
			}
		}

		[System.Serializable]
		public class AdvancedMultiplier : Multiplier {
			[SerializeField] private float valueRate = 0;
			[SerializeField] private bool isMultiplier = true;

			public override float GetValue(int level) {
				var value = Mathf.Pow(valueRate, level);
				value = isMultiplier ? initValue * value : initValue / value;
				var roundedValue = System.Math.Round(value, decimalPlaces);
				return (float)roundedValue;
			}
		}

		[System.Serializable]
		public class AdvancedMaxLevel {
			[SerializeField] public UpgradePreset upgradePreset = null;
			[SerializeField] private int initValue = 2;
			[SerializeField] private int multiplierValue = 2;

			public int GetLevel() {
				return initValue + (upgradePreset.GetLevel() - 1) * multiplierValue;
			}
		}

		[Header("Settings")]
		[SerializeField] public UpgradeType upgradeType = UpgradeType.None;

		[Header("Visual")]
		[TextArea] public string panelDisplayName = string.Empty;

		[Header("Values")]
		[SerializeField] public Multiplier cost = default;
		[SerializeField] public AdvancedMultiplier value = default;

		[Header("Levels")]
		[SerializeField] public int defaultLevel = 0;
		[SerializeField] private bool simpleLevelSystem = true;
		[SerializeField] private int maxLevel = 0;
		[SerializeField] public AdvancedMaxLevel advancedMaxLevel = null;

		public System.Action OnLevelUp;

		[Header("Save System")]
		private const string SAVE_NAME = "_LEVEL";

		#region SAVE
		public virtual void ResetSave() => SetLevel(defaultLevel);

		public int GetLevel() => PlayerPrefs.GetInt(GetKey(), defaultLevel);

		public void SetLevel(int level) => PlayerPrefs.SetInt(GetKey(), level);

		private protected string GetKey() => $"{upgradeType}{SAVE_NAME}";
		#endregion

		#region  VALUE
		public float GetValue() => Value(GetLevel());

		public float Value(int level) => value.GetValue(level);
		#endregion

		#region  COST
		public float Cost() => Cost(GetLevel());

		public float Cost(int level) => cost.GetValue(level);

		public float NextLevelCost() => Cost(GetLevel() + 1);
		#endregion

		#region LEVELS AND UPGRADES

		public bool CanUpgrade(float currentCoins) {
			if (IsMaxLevel()) return false;
			return CanAfford(currentCoins);
		}

		public bool CanUpgrade() => CanUpgrade(Players.PlayerWallet.instance.GetMoney());

		public bool CanAfford(float currentCoins) => currentCoins >= NextLevelCost();

		public bool IsMaxLevel() => GetLevel() >= GetMaxLevel();

		public void LevelUp() => ModifyLevel(1);

		public void LevelDown() => ModifyLevel(-1);

		public virtual void ModifyLevel(int modifier) {
			int level = GetLevel();
			level = Mathf.Clamp(level + modifier, defaultLevel, GetMaxLevel());
			SetLevel(level);
			OnLevelUp?.Invoke();
		}

		public int GetMaxLevel() => simpleLevelSystem ? maxLevel : advancedMaxLevel.GetLevel();

		#endregion
	}
}