using UnityEngine;
using UnityEngine.UIElements;
using Upgrades;

namespace Weapons {
	[CreateAssetMenu(fileName = "Weapon X - Tree", menuName = "SO/Weapons/Weapon Upgrade Tree", order = 1)]
	public class WeaponUpgradeTree : ScriptableObject {

		[Header("Component")]
		public WeaponStats stats = null;

		[Header("Upgrades")]
		public UpgradeValue upgradeCost = new UpgradeValue();
		public UpgradeValue bulletSpeed = new UpgradeValue(1, 10);
		public UpgradeValue shootSpeed = new UpgradeValue(2, 1);
		public UpgradeValue powerDamage = new UpgradeValue(1, 10);
		public UpgradeValue radius = new UpgradeValue(0, 0);
	}


	[System.Serializable]
	public class UpgradeValue {
		public WeaponStats stats;

		public AnimationCurve curve;
		public float minValue;
		public float maxValue;

		public UpgradeValue() {
			curve = new AnimationCurve() {
				keys = new Keyframe[] {
				  new Keyframe(0, 0),
				  new Keyframe(1, 1)
				},
			};
			minValue = 0;
			maxValue = 1;
		}

		public UpgradeValue(float minValue, float maxValue) {
			curve = new AnimationCurve() {
				keys = new Keyframe[] {
				  new Keyframe(0, 0),
				  new Keyframe(1, 1)
				},
			};
			this.minValue = minValue;
			this.maxValue = maxValue;
		}

		public float GetValue() {
			int currentLevel = CurrentLevel();
			return GetValueAtLevel(currentLevel);
		}

		public float GetValue(int offset) {
			int currentLevel = CurrentLevel() + offset;
			return GetValueAtLevel(currentLevel);
		}

		public float GetValueAtLevel(int currentLevel) {
			float time = GetTime(stats.minLevel, stats.maxLevel, currentLevel);
			float value = Mathf.Lerp(minValue, maxValue, curve.Evaluate(time));
			return (float)System.Math.Round(value, 2);
		}

		public int CurrentLevel() => stats.speedUpgrade.GetLevel() + stats.minLevel;

		private float GetTime(int minLevel, int maxLevel, int currentLevel) {
			float max = maxLevel - minLevel;
			float current = currentLevel - minLevel;
			return current / max;
		}

	}
}