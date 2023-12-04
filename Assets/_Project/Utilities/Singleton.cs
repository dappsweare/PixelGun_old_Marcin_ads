using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
	private static T _instance = null;

	static public T instance {
		get {
			if(_instance == null){
				T[] array = Resources.FindObjectsOfTypeAll<T>();
				for(int a = 0; a < array.Length; a++) {
					if(array[a].gameObject.scene.name != null)
						return _instance = array[a];
				}
				
				return null;
			}
			else return _instance;
		}
	}

	private protected virtual void Awake(){
		_instance = this as T;
	}
}