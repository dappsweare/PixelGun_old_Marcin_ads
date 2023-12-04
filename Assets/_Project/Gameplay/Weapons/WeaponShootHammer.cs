using Level;
using UnityEngine;
using UnityEngine.UIElements;
using static Weapons.WeaponStats;

namespace Weapons {
	public class WeaponShootHammer : WeaponShoot {

		[System.Serializable]
		private struct LightingSettings {

			[Header("Color")]
			public Color colorStart;
			public Color colorEnd;

			[Header("Settings")]
			public float width;
			public Vector2 xRangeStart;
			public Vector2 xRangeUpdate;
		}

		[Header("Hit")]
		[SerializeField] private GameObject hitParticles = null;

		[Header("Lightning")]
		[SerializeField] private LineRenderer lineRenderer = null;
		[SerializeField] private LayerMask pixelLayerMask = ~0;
		[SerializeField] private LightingSettings[] lightingSettings = null;

		private LineRenderer[] currentLighting = null;
		private bool containsLighting = false;
		private Coroutine autoDestroy = null;

		private void Start() {
			currentLighting = new LineRenderer[3];
		}

		private void Update() {
			UpdateCurrentLighting();
		}

		private protected override void Shoot() {
			if (Physics.Raycast(transform.position, transform.up, out RaycastHit hit, float.PositiveInfinity, pixelLayerMask.value)) {
				CreateLighting(hit.point, true, hit);
			} else {
				CreateLighting(transform.up * 100f, false);
			}

			CreateEffects();
			ResetTimer();
		}

		private void CreateLighting(Vector3 hitPoint, bool didHit, RaycastHit hit = default) {
			if (containsLighting) {
				RemoveLighting();
				StopCoroutine(autoDestroy);
			}

			for (int a = 0; a < lightingSettings.Length; a++)
				InstantiateLighting(hitPoint, a);

			if (didHit) {
				CreateHit(hit);
				DealDamage(hit);
			}
			AutoDestroyLighting();
			containsLighting = true;
		}

		private void UpdateCurrentLighting() {
			for (int a = 0; a < currentLighting.Length; a++) {
				var lighting = currentLighting[a];
				if (lighting == null) continue;

				int points = lighting.positionCount;
				Vector2 xRange = lightingSettings[a].xRangeUpdate;
				for (int b = 0; b < points; b++) {
					Vector3 point = lighting.GetPosition(b);
					point.x += Random.Range(xRange.x, xRange.y);
					lighting.SetPosition(b, point);
				}
			}
		}

		private void RemoveLighting() {
			for (int a = 0; a < currentLighting.Length; a++) {
				Destroy(currentLighting[a].gameObject);
			}
			containsLighting = false;
		}

		private void AutoDestroyLighting() {
			autoDestroy = StartCoroutine(Invoker.WaitAndInvoke(() => {
				if (containsLighting) {
					RemoveLighting();
				}
			}, .5f));
		}

		private void InstantiateLighting(Vector3 hit, int index) {
			LineRenderer newLighting = GameObjects.GOInstantiate(lineRenderer, "Lightning", transform);
			newLighting.sortingOrder = index;
			newLighting.startWidth = lightingSettings[index].width;
			newLighting.startColor = lightingSettings[index].colorStart;
			newLighting.endColor = lightingSettings[index].colorEnd;

			Vector2 xRange = lightingSettings[index].xRangeStart;
			Vector3 pointA = transform.position;
			Vector3 pointB = hit;
			int nodes = 25;
			Vector3[] points = UnityExtension.CreatePath(pointA, pointB, nodes);
			newLighting.positionCount = nodes;

			for (int a = 0; a < points.Length; a++) {
				Vector3 point = points[a];
				point.x += Random.Range(xRange.y, xRange.x);
				newLighting.SetPosition(a, point);
			}

			currentLighting[index] = newLighting;
		}

		private void CreateHit(RaycastHit hit) {
			var hitParticle = GameObjects.GOInstantiate(hitParticles, hit.point, Vector3.zero);
			Destroy(hitParticle.gameObject, .25f);
		}

		private void DealDamage(RaycastHit hit) {
			float damage = weaponTree.powerDamage.GetValue(-1);
			float radius = weaponTree.radius.GetValue(-1);
			bool singleHit = weapon.stats.shootSettings.singleHit;
			RadiusHitType radiusHitType = weapon.stats.shootSettings.radiusHitType;
			Vector2 radiusDamageMultiplier = weapon.stats.shootSettings.radiusDamageMultiplier;

			DamageInfo damageInfo = new DamageInfo(0, damage, singleHit, radius, radiusHitType, radiusDamageMultiplier);
			WeaponDamageController.instance.DealDamage(hit, damageInfo);
		}
	}
}