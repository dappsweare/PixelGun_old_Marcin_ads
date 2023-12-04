using UnityEngine;

namespace UI {
    public class UIComponentButtonAudio : UIComponentButtonToggle {

        private void OnEnable() {
            var currentAudio = Global.GameManager.instance.GetAudio();
            Toggle(currentAudio);
        }

        public override void OnClick() {
            var currentAudio = Global.GameManager.instance.GetAudio();
            Global.GameManager.instance.SetAudio(!currentAudio);
            Toggle(!currentAudio);
        }
    }
}