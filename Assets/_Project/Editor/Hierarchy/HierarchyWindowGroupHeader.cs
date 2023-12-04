using UnityEngine;
using UnityEditor;

/// <summary>
/// Hierarchy Window Group Header
/// http://diegogiacomelli.com.br/unitytips-hierarchy-window-group-header
/// </summary>
[InitializeOnLoad]
public static class HierarchyWindowGroupHeader {

    private static readonly string HEADER_BIG_TAG = "---";
    private static readonly Color HEADER_BIG_COLOR = new Color(0.176f, 0.176f, 0.176f);
    private static readonly string HEADER_MID_TAG = "--";
    private static readonly Color HEADER_MID_COLOR = new Color(0.434f, 0.434f, 0.434f);

    static HierarchyWindowGroupHeader() {
        EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
    }

    static void HierarchyWindowItemOnGUI(int instanceID, Rect selectionRect) {
        var gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

        if (gameObject != null) {
            if (gameObject.name.StartsWith(HEADER_BIG_TAG, System.StringComparison.Ordinal)) {
                Draw(gameObject, selectionRect, HEADER_BIG_COLOR);
            } else if (gameObject.name.StartsWith(HEADER_MID_TAG, System.StringComparison.Ordinal)) {
                Draw(gameObject, selectionRect, HEADER_MID_COLOR);
            }
        }
    }

    private static void Draw(GameObject gameObject, Rect selectionRect, Color color) {
        EditorGUI.DrawRect(selectionRect, color);
        EditorGUI.DropShadowLabel(selectionRect, gameObject.name.Replace("-", "").ToUpperInvariant());
    }
}