using UnityEngine;
using UnityEditor;

namespace Weapons {
	[CustomEditor(typeof(WeaponUpgradeSkin), true)]
	[CanEditMultipleObjects]
	public class WeaponUpgradeSkinEditor : Editor {

		private WeaponUpgradeSkin skin = null;

		private void OnEnable() {
			skin = (WeaponUpgradeSkin)target;
		}

		public override void OnInspectorGUI() {
			base.OnInspectorGUI();
			GUILayout.Label(
				$"{skin.weaponStats.minLevel} - {skin.weaponStats.maxLevel}",
				Dapps.EditorUtilities.BoldLabelStyle(Color.white, TextAnchor.MiddleCenter)
			);
		}
	}
}