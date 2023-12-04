using UnityEditor;
using UnityEngine;

namespace Weapons {
    [CanEditMultipleObjects]
    [CustomEditor(typeof(WeaponEffects), true)]
    public class WeaponEffectsEditor : Editor {

		private WeaponEffects effects = null;

		private void OnEnable() => effects = (WeaponEffects)target;

		public override void OnInspectorGUI() {
			base.OnInspectorGUI();

			Dapps.EditorUtilities.HorizontalLine(5);

			Dapps.EditorUtilities.Horizontal(() => {
				if (GUILayout.Button("Preview - True")) effects.SetPreview(true);
				if (GUILayout.Button("Preview - False")) effects.SetPreview(false);
			});
		}

	}
}