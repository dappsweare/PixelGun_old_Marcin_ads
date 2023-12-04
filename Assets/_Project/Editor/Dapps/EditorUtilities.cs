using System;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

namespace Dapps {
    public static class EditorUtilities {

        // https://github.com/halak/unity-editor-icons
        // https://gist.github.com/MadLittleMods/ea3e7076f0f59a702ecb

        public static void Vertical(Action content, bool isBox = false) {
            EditorGUILayout.BeginVertical(isBox ? "Box" : GUIStyle.none);
            content?.Invoke();
            EditorGUILayout.EndVertical();
        }

        public static void Vertical(Action content, bool isBox, float width, float height) {

            EditorGUILayout.BeginVertical(isBox ? "Box" : GUIStyle.none, GUILayout.Width(width), GUILayout.Height(height));
            content?.Invoke();
            EditorGUILayout.EndVertical();
        }


        public static void Horizontal(Action content, bool isBox = false) {
            EditorGUILayout.BeginHorizontal(isBox ? "Box" : GUIStyle.none, GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false));
            content?.Invoke();
            EditorGUILayout.EndHorizontal();
        }

        public static void Horizontal(Action content, bool isBox, float width, float height, string hexColor = "") {

            GUIStyle style = new GUIStyle(isBox ? "Box" : GUIStyle.none);
            Texture2D texture = new Texture2D(1, 1);
            if (ColorUtility.TryParseHtmlString("#" + hexColor, out Color color)) {
                texture.SetPixel(0, 0, color);
                texture.Apply();
                style.normal.background = texture;
            }

            EditorGUILayout.BeginHorizontal(style, GUILayout.Width(width), GUILayout.ExpandWidth(false), GUILayout.Height(height), GUILayout.ExpandHeight(false));
            content?.Invoke();
            EditorGUILayout.EndHorizontal();
        }


        public static void DisabledGroup(Action content, bool value) {
            EditorGUI.BeginDisabledGroup(value);
            content?.Invoke();
            EditorGUI.EndDisabledGroup();
        }


        public static void HorizontalLine(int height = 30) => HorizontalLine("", height);

        public static void HorizontalLine(string content, int height = 30) {
            GUILayout.Space(5);
            GUILayout.Label(content.ToUpper(), HorizontalLineStyle(height), GUILayout.Height(height), GUILayout.ExpandWidth(true));
            GUILayout.Space(5);
        }


        public static void DappsMenu(string menuTitle) {
            GUIContent content = new GUIContent() {
                image = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/_Project/Editor/Dapps/Icons/DappsLogo.png", typeof(Texture2D)),
                text = "<b>APPS</b>"
            };
            GUIStyle style = LabelStyle(Color.white, TextAnchor.MiddleCenter);
            style.fontSize = 24;

            GUILayout.Label(content, style, GUILayout.Height(70));
            GUILayout.Label(menuTitle, BoldLabelStyle(Color.white), GUILayout.Height(20));
            GUILayout.Space(10);
        }


        public static GUIStyle HorizontalLineStyle(int height = 30) {
            return new GUIStyle("ProgressBarBack") {
                alignment = TextAnchor.MiddleCenter,
                richText = true,
                wordWrap = true,
                fontSize = height / 2,
                margin = new RectOffset(5, 5, 5, 5)
            };
        }

        public static void Header(string text) {
            GUILayout.Label($"<b>{text}</b>", Dapps.EditorUtilities.LabelStyle(new Color(.769f, .769f, .769f), TextAnchor.MiddleLeft));
        }

        public static GUIStyle LabelStyle() => LabelStyle(Color.white, TextAnchor.MiddleCenter);

        public static GUIStyle LabelStyle(TextAnchor textAnchor) => LabelStyle(Color.white, textAnchor);

        public static GUIStyle LabelStyle(Color text, TextAnchor textAnchor) {
            GUIStyle style = new GUIStyle("label") {
                alignment = textAnchor,
                richText = true,
                wordWrap = true
            };

            style.normal.textColor = text;
            style.hover.textColor = text;
            style.active.textColor = text;
            style.focused.textColor = text;

            return style;
        }


        public static GUIStyle BoldLabelStyle() => BoldLabelStyle(Color.white);

        public static GUIStyle BoldLabelStyle(Color text) => BoldLabelStyle(text, TextAnchor.MiddleCenter);

		public static GUIStyle BoldLabelStyle(Color text, TextAnchor textAnchor) {
			GUIStyle style = new GUIStyle(EditorStyles.boldLabel) {
				alignment = textAnchor,
				richText = true,
				wordWrap = true
			};

			style.normal.textColor = text;
			style.hover.textColor = text;
			style.active.textColor = text;
			style.focused.textColor = text;

			return style;
		}


		public static GUIStyle TextPreview(Color text) {
            GUIStyle style = new GUIStyle("Box") {
                alignment = TextAnchor.MiddleCenter,
                richText = true,
                wordWrap = true
            };

            style.normal.textColor = text;
            style.hover.textColor = text;
            style.active.textColor = text;
            style.focused.textColor = text;

            return style;
        }


        public static GUIStyle CustomStyle(string style) => new GUIStyle(style);

        public static GUIStyle CustomStyle(string style, Color text) {
            GUIStyle newStyle = CustomStyle(style);

            newStyle.normal.textColor = text;
            newStyle.hover.textColor = text;
            newStyle.active.textColor = text;
            newStyle.focused.textColor = text;

            return newStyle;
        }

        public static GUIStyle CustomStyle(string style, Color text, TextAnchor alignment) {
            GUIStyle newStyle = CustomStyle(style, text);
            newStyle.alignment = alignment;

            return style;
        }


    }
}
#endif