using UnityEngine;

public class GlobalSceneInitializer {
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void OnBeforeSceneLoadRuntimeMethod(){
		var global = GameObjects.GOInstantiate(Resources.Load("_Global"), Vector3.zero, Vector3.zero, null);
		Object.DontDestroyOnLoad(global);
	}
}
