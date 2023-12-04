using UnityEditor;
using UnityEngine;
using static Weapons.WeaponStats;

namespace Weapons {
	[CustomPropertyDrawer(typeof(WeaponStats.ShootSettings))]
	public class ShootSettingsDrawer : PropertyDrawer {

		public class DrawData {
			public SerializedProperty property;
			public Rect rect;

			public DrawData(Rect position, SerializedProperty property, string path, int lineIndex) {
				rect = new Rect(position.min.x, position.min.y + lineIndex++ * EditorGUIUtility.singleLineHeight, position.size.x, EditorGUIUtility.singleLineHeight);
				this.property = property.FindPropertyRelative(path);
			}
		}

		private int linesAmount = 0;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			linesAmount = 0;
			DrawData bounce = new DrawData(position, property, "bounce", 0);
			DrawData chaosMode = new DrawData(position, property, "chaosMode", 0);
			bounce.rect.width /= 2;
			chaosMode.rect.width /= 2;
			chaosMode.rect.x += chaosMode.rect.width;

			EditorGUI.PropertyField(bounce.rect, bounce.property);
			EditorGUI.PropertyField(chaosMode.rect, chaosMode.property);

			linesAmount++;

			DrawData singleHit = new DrawData(position, property, "singleHit", 1);
			singleHit.rect.width /= 2;
			EditorGUI.PropertyField(singleHit.rect, singleHit.property);
			linesAmount++;

			if (!singleHit.property.boolValue) {
				DrawData radiusHitType = new DrawData(position, property, "radiusHitType", 1);
				radiusHitType.rect.width /= 2;
				radiusHitType.rect.x += radiusHitType.rect.width;
				EditorGUI.PropertyField(radiusHitType.rect, radiusHitType.property, GUIContent.none);
				if (radiusHitType.property.enumValueIndex == 1) {
					DrawData radiusDamageMultiplier = new DrawData(position, property, "radiusDamageMultiplier", 2);
					radiusDamageMultiplier.rect.width /= 2;
					radiusDamageMultiplier.rect.x += radiusDamageMultiplier.rect.width;
					EditorGUI.PropertyField(radiusDamageMultiplier.rect, radiusDamageMultiplier.property, GUIContent.none);
				}
			}

			linesAmount++;

			DrawData shootType = new DrawData(position, property, "shootType", 3);
			EditorGUI.PropertyField(shootType.rect, shootType.property);
			linesAmount++;

			if (shootType.property.enumValueIndex == 1) {
				DrawData burstDelay = new DrawData(position, property, "burstDelay", 4);
				DrawData burstAmount = new DrawData(position, property, "burstAmount", 4);
				burstDelay.rect.width /= 2;
				burstAmount.rect.width /= 2;
				burstAmount.rect.x += burstAmount.rect.width;

				EditorGUI.PropertyField(burstDelay.rect, burstDelay.property, new GUIContent("Delay"));
				EditorGUI.PropertyField(burstAmount.rect, burstAmount.property, new GUIContent("Amount"));
				linesAmount++;
			} else if (shootType.property.enumValueIndex == 2) {
				DrawData shotgunAmount = new DrawData(position, property, "shotgunAmount", 4);
				DrawData shotgunOffset = new DrawData(position, property, "shotgunOffset", 4);
				shotgunAmount.rect.width /= 2;
				shotgunOffset.rect.width /= 2;
				shotgunOffset.rect.x += shotgunOffset.rect.width;

				EditorGUI.PropertyField(shotgunAmount.rect, shotgunAmount.property, new GUIContent("Amount"));
				EditorGUI.PropertyField(shotgunOffset.rect, shotgunOffset.property, new GUIContent("Offset"));
				linesAmount++;
			} else if (shootType.property.enumValueIndex == 3) {
				DrawData shotgunAndBurstAmount = new DrawData(position, property, "shotgunAndBurstAmount", 4);
				DrawData shotgunAndBurstDelay = new DrawData(position, property, "shotgunAndBurstDelay", 4);
				DrawData shotgunAndBurstOffset = new DrawData(position, property, "shotgunAndBurstOffset", 5);
				shotgunAndBurstAmount.rect.width /= 2;
				shotgunAndBurstDelay.rect.width /= 2;
				shotgunAndBurstDelay.rect.x += shotgunAndBurstDelay.rect.width;

				EditorGUI.PropertyField(shotgunAndBurstAmount.rect, shotgunAndBurstAmount.property, new GUIContent("Amo	unt"));
				EditorGUI.PropertyField(shotgunAndBurstDelay.rect, shotgunAndBurstDelay.property, new GUIContent("Delay"));
				EditorGUI.PropertyField(shotgunAndBurstOffset.rect, shotgunAndBurstOffset.property, new GUIContent("Offset"));
				linesAmount += 2;
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			return (EditorGUIUtility.singleLineHeight + .5f) * linesAmount;
		}
	}
}