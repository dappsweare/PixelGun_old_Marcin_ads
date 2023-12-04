using UnityEngine;

public static class GameObjects {
	public static GameObject NewGameObject(string name, Vector3 position = default, Vector3 eulerAngles = default, Transform parent = null, bool localizeTransform = false){
		GameObject gameObject = new GameObject(name);
		gameObject.transform.parent = parent;
		gameObject.transform.position = parent && localizeTransform ? parent.position + parent.rotation * position : position;
		gameObject.transform.rotation = parent && localizeTransform ? parent.rotation * Quaternion.Euler(eulerAngles) : Quaternion.Euler(eulerAngles);
		return gameObject;
	}

	public static GameObject NewGameObject(string name, Transform parent, bool localizeTransform = false){
		return NewGameObject(name, default, default, parent, localizeTransform);
	}

	public static T GOInstantiate<T>(T prefab, string name, Vector3 position = default, Vector3 eulerAngles = default, Transform parent = null, bool localizeTransform = false) where T : Object {
		T gameObject =
			Object.Instantiate(
				prefab,
				parent && localizeTransform ? parent.position + parent.rotation * position : position,
				parent && localizeTransform ? parent.rotation * Quaternion.Euler(eulerAngles) : Quaternion.Euler(eulerAngles),
				parent
			);

		gameObject.name = name;
		return gameObject;
	}

	public static T GOInstantiate<T>(T prefab, string name, Transform parent = null, bool localizeTransform = false) where T : Object {
		return GOInstantiate(prefab, name, default, default, parent, localizeTransform);
	}

	public static T GOInstantiate<T>(T prefab, Vector3 position = default, Vector3 eulerAngles = default, Transform parent = null, bool localizeTransform = false) where T : Object {
		return GOInstantiate(prefab, prefab.name, position, eulerAngles, parent, localizeTransform);
	}

	public static T GOInstantiate<T>(T prefab, Transform parent = null, bool localizeTransform = false) where T : Object {
		return GOInstantiate(prefab, prefab.name, default, default, parent, localizeTransform);
	}

	public static T GOInstantiate<T>(T prefab) where T : Component {
		return GOInstantiate(prefab, prefab.name, default, default);
	}

	public static RectTransform UIGOInstantiate(GameObject prefab, string name, Vector2 anchoredPosition, Vector2 sizeDelta, Vector3 rotation, Transform parent = null, int childIndex = -1){
		GameObject gameObject = Object.Instantiate(prefab, Vector3.zero, Quaternion.Euler(rotation), parent);
		gameObject.name = name;
		RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
		rectTransform.anchoredPosition = anchoredPosition;
		rectTransform.sizeDelta = sizeDelta;
		if(childIndex >= 0)
			gameObject.transform.SetSiblingIndex(childIndex);
		return rectTransform;
	}
}