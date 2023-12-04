
namespace UI {
	public class UIComponentButtonJoystick : UIComponentButtonToggle {

		private void OnEnable() {
			var currentValue = Global.GameManager.instance.joystickController;
			Toggle(currentValue);
		}

		public override void OnClick() {
			var currentValue = Global.GameManager.instance.joystickController;
			Global.GameManager.instance.joystickController = !currentValue;
			Toggle(!currentValue);
		}
	}
}