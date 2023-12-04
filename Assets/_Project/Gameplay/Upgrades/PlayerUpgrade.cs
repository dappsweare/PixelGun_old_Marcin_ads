using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Upgrades {
	[System.Serializable]
	public class PlayerUpgrade {

		[Header("Settings")]
		[SerializeField] private protected UpgradePreset upgradePreset = null;

		public UpgradePreset UpgradePreset() => upgradePreset;
	}

	[System.Serializable]
	public class PlayerUpgradeFloat : PlayerUpgrade {

		[Header("Settings")]
		[SerializeField] private float defaultValue = 0f;

		public float GetValue() => GetValue(upgradePreset.GetLevel());

		public float GetValue(int level) => upgradePreset.Value(level) + defaultValue;
	}

	[System.Serializable]
	public class PlayerUpgradeInt : PlayerUpgrade {

		[Header("Settings")]
		[SerializeField] private int defaultValue = 0;

		public int GetValue() => GetValue(upgradePreset.GetLevel());

		public int GetValue(int level) => Mathf.RoundToInt(upgradePreset.Value(level) + defaultValue);
	}
}