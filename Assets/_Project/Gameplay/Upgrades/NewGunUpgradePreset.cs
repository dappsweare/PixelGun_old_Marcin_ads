using UnityEngine;
using Weapons;

namespace Upgrades {
	[CreateAssetMenu(fileName = "New Gun Preset", menuName = "SO/Upgrades/New Gun Preset", order = 2)]
	public class NewGunUpgradePreset : UpgradePreset {

		public override void ModifyLevel(int modifier) {
			base.ModifyLevel(modifier);

			if (Application.isPlaying) {
				WeaponController.instance.UnlockNewWeapon();
				OnLevelUp?.Invoke();
			}
		}
	}
}