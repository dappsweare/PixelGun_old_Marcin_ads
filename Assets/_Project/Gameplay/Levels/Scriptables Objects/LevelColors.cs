using UnityEngine;


namespace Level {
    [System.Serializable]
    public class LevelColors {
        public Color pixelColor = Color.white;
        public Color targetColor = Color.white;

        public LevelColors(Color pixelColor) {
            this.pixelColor = pixelColor;
            Color targetColor = pixelColor;

            if (Equals(pixelColor, Color.black)) {
                ColorUtility.TryParseHtmlString("#2F2F2F", out Color newTargetColor);
                targetColor = newTargetColor;
            }
            this.targetColor = targetColor;

        }
    }
}