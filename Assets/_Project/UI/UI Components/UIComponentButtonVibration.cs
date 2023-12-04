
namespace UI {
    public class UIComponentButtonVibration : UIComponentButtonToggle {

        private void OnEnable() {
            var currentHaptics = Global.GameManager.instance.GetHaptics();
            Toggle(currentHaptics);
        }

        public override void OnClick() {
            var currentHaptics = Global.GameManager.instance.GetHaptics();
            Global.GameManager.instance.SetHaptics(!currentHaptics);
            Toggle(!currentHaptics);
        }
    }
}