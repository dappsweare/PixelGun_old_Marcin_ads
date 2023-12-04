using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Level {
	[CustomEditor(typeof(PixelController))]
	public class PixelControllerEditor : Editor {

		public override void OnInspectorGUI() {
			base.OnInspectorGUI();

			GUILayout.Space(10);
			Dapps.EditorUtilities.Header("Buttons");

			Dapps.EditorUtilities.Horizontal(() => {
				GUILayout.FlexibleSpace();

				if (GUILayout.Button("Create", GUILayout.Width(70), GUILayout.Height(30))) Create();

				if (GUILayout.Button("Rebuild", GUILayout.Width(70), GUILayout.Height(30))) Rebuild();

				if (GUILayout.Button("Clear", GUILayout.Width(70), GUILayout.Height(30))) Clear();

				GUILayout.FlexibleSpace();
			});
		}

		private void Create() {
			Clear();
			GetPixelController().ForEach((x) => {
				x.CreateLevel();
				EditorUtility.SetDirty(x);
			});

		}

		private void Rebuild() {
			var pixelControllers = GetPixelController();
			var amount = pixelControllers.Count;
			float[] yPositions = new float[amount];
			for (int a = 0; a < amount; a++)
				yPositions[a] = pixelControllers[a].consturctor.pixelParent.position.y;

			Clear();

			for (int a = 0; a < amount; a++) {
				var pixelController = pixelControllers[a];
				pixelController.CreateLevel();
				var position = pixelController.consturctor.pixelParent.position;
				position.y = yPositions[a];
				pixelController.consturctor.pixelParent.position = position;

				EditorUtility.SetDirty(pixelController);
			}
		}

		private void Clear() {
			GetPixelController().ForEach((x) => {
				x.ClearLevel();
				EditorUtility.SetDirty(x);
			});
		}

		private List<PixelController> GetPixelController() {
			var pixelControllers = new List<PixelController>();
			for (int a = 0; a < targets.Length; a++) {
				if (targets[a] is PixelController pixelController) {
					pixelControllers.Add(pixelController);
				}
			}
			return pixelControllers;
		}
	}
}