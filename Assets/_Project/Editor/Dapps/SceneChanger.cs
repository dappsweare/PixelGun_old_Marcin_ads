using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.SceneManagement;

namespace Dapps {
    public class SceneChanger : EditorWindow {

        private class SceneData {
            public string guid;
            public string path;
            public SceneAsset sceneAsset;

            public SceneData(string guid, string path, SceneAsset sceneAsset) {
                this.guid = guid;
                this.path = path;
                this.sceneAsset = sceneAsset;
            }
        }

        private const string MAIN = "DAPPS";
        private const string WINDOW = MAIN + " / Scene Changer";

        private Vector2 scrollData = Vector2.zero;
        private List<SceneData> currentSceneData = null;

        [MenuItem(WINDOW)]
        public static void ShowWindow() {
            SceneChanger editor = (SceneChanger)EditorWindow.GetWindow(typeof(SceneChanger));
            editor.titleContent = new GUIContent("Scene Changer");
        }

        private void OnEnable() {
            RefreshScenes();
        }

        private void OnGUI() {
            EditorUtilities.DappsMenu("Scene Changer");

            DrawMenu();
            DrawScenes();
        }

        private void DrawMenu() {
            EditorUtilities.Horizontal(() => {
                Button("Refresh", () => {
                    RefreshScenes();
                });

                Button("All Scenes", () => {
                    currentSceneData = GetScenes(string.Empty);
                });

                Button("All Scenes - Level", () => {
                    currentSceneData = GetScenes("Level - ");
                });
            });

            void Button(string title, System.Action action) {
                if (GUILayout.Button(title, GUILayout.Height(50))) {
                    action?.Invoke();
                }
            }
        }

        private void DrawScenes() {
            EditorUtilities.Vertical(() => {
                scrollData = GUILayout.BeginScrollView(scrollData);
                for (int a = 0; a < currentSceneData.Count; a++)
                    DrawScene(currentSceneData[a]);
                GUILayout.EndScrollView();
            });
        }

        private void RefreshScenes() => currentSceneData = GetScenes(string.Empty);

        private void DrawScene(SceneData data) {
            if (data == null) return;
            string name = data.sceneAsset.name;
            string path = data.path;

            EditorUtilities.Horizontal(() => {
                EditorGUI.BeginDisabledGroup(UnityEngine.SceneManagement.SceneManager.GetActiveScene().path == path);
                if (GUILayout.Button(name, GUILayout.Height(30))) {
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                        EditorSceneManager.OpenScene(path);
                }
                EditorGUI.EndDisabledGroup();

                if (GUILayout.Button(EditorGUIUtility.IconContent("AreaLight Gizmo"), GUILayout.Width(25), GUILayout.Height(30))) {
                    var asset = AssetDatabase.LoadMainAssetAtPath(path);
                    EditorGUIUtility.PingObject(asset);
                }

            });
        }

        private List<SceneData> GetScenes(string scenePrefix) {
            List<SceneData> assets = new List<SceneData>();
            string[] guids = AssetDatabase.FindAssets("t:SceneAsset", null);
            for (int i = 0; i < guids.Length; i++) {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                SceneAsset asset = AssetDatabase.LoadAssetAtPath<SceneAsset>(assetPath);
                if (asset == null) continue;
                if (!string.IsNullOrEmpty(scenePrefix) && !asset.name.StartsWith(scenePrefix))
                    continue;

                assets.Add(new SceneData(guids[i], assetPath, asset));
            }

            return assets;
        }
    }
}