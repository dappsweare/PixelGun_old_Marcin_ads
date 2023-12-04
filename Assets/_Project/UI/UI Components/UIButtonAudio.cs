using UnityEngine;
using UnityEngine.UI;

namespace UI {
    [RequireComponent(typeof(Button))]
    public class UIButtonAudio : MonoBehaviour {

		[Header("Settings")]
		[SerializeField] private Global.AudioKey audioKey = Global.AudioKey.UI_Button;

		private void Start() {
			Button button = GetComponent<Button>();
			button.onClick.AddListener(PlayAudio);
		}

		public void PlayAudio() {
			Global.AudioManager.instance.Play(audioKey);
		}
	}
}