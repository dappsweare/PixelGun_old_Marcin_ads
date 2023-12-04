using System;
using UnityEngine;
using UnityEngine.EventSystems;

public static class UnityExtension {

	public static void DestroyAllChildren(this Transform transform) {
		for (int a = transform.childCount - 1; a >= 0; a--) {
			transform.GetChild(a).gameObject.DestroyInEdtiorOrRuntime();
		}
	}

	public static void DestroyInEdtiorOrRuntime(this UnityEngine.Object obj) {
		if (Application.isPlaying)
			UnityEngine.Object.Destroy(obj);
		else
			UnityEngine.Object.DestroyImmediate(obj);
	}

	public static bool IsOverUI() {
#if UNITY_EDITOR
		return EventSystem.current.IsPointerOverGameObject();
#else
		return Input.touchCount > 0 ? EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) : false;
#endif
	}

	public static Color ModifyColor(this Color color, float value) {
		return new Color(color.r + value, color.g + value, color.b + value);
	}

	public static void ModifyColor(this MeshRenderer renderer, Color color) {
		var materials = renderer.materials;
		for (int a = 0; a < materials.Length; a++) {
			materials[a].color = color;
		}
		renderer.materials = materials;
	}

	public static void ModifyColor(this MeshRenderer renderer, params Color[] colors) {
		var materials = renderer.materials;
		for (int a = 0; a < materials.Length; a++)
			materials[a].color = colors[a];
		renderer.materials = materials;
	}

	public static void ModifyColor(this MeshRenderer renderer, int meshIndex, Color color) {
		var materials = renderer.materials;
		for (int a = 0; a < materials.Length; a++) {
			if (meshIndex == a) {
				materials[a].color = color;
				break;
			}
		}
		renderer.materials = materials;
	}

	public static Vector3 Clamp(this Vector3 value, Vector3 min, Vector3 max) {
		return new Vector3(
			Mathf.Clamp(value.x, min.x, max.x),
			Mathf.Clamp(value.y, min.y, max.y),
			Mathf.Clamp(value.z, min.z, max.z)
		);
	}

	public static string GetPercentage(float value) {
		value *= 100f;
		float percentage = (float)Math.Round(value, 0);
		return $"{percentage}%";
	}
	
	public static int Repeat(int t, int length) {
		t %= length;
		return t >= 0 ? t : t + length;
	}

	public static Vector3[] CreatePath(Vector3 pointA, Vector3 pointB, Vector3 pointC, int nodes) {
		Vector3[] points = new Vector3[nodes];
		for (int a = 1; a < nodes + 1; a++) {
			float t = a / (float)nodes;
			Vector3 point = Curve(pointA, pointB, pointC, t);
			points[a - 1] = point;
		}
		return points;
	}

	public static Vector3[] CreatePath(Vector3 pointA, Vector3 pointB, int nodes) {
		Vector3[] points = new Vector3[nodes];
		for (int a = 1; a < nodes + 1; a++) {
			float t = a / (float)nodes;
			Vector3 point = Vector3.Lerp(pointA, pointB, t);
			points[a - 1] = point;
		}
		return points;
	}

	private static Vector3 Curve(Vector3 a, Vector3 b, Vector3 c, float t) {
		Vector3 p0 = Vector3.Lerp(a, b, t);
		Vector3 p1 = Vector3.Lerp(b, c, t);
		return Vector3.Lerp(p0, p1, t);
	}

	public static float Remap(float input, float oldLow, float oldHigh, float newLow, float newHigh) {
		float t = Mathf.InverseLerp(oldLow, oldHigh, input);
		return Mathf.Lerp(newLow, newHigh, t);
	}

	public static Vector3 GetEulerByDirection(Vector3 direction, float zOffset) {
		float zEuler = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		return new Vector3(0f, 0f, zEuler + zOffset);
	}
}
