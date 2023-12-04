using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using Upgrades;
using System;
using Weapons;

namespace Dapps {
    public class CheatPanel : EditorWindow {

        private const string MAIN = "DAPPS";
        private const string WINDOW = MAIN + " / Cheat Panel";

        [MenuItem(WINDOW)]
        public static void ShowWindow() {
            CheatPanel editor = (CheatPanel)EditorWindow.GetWindow(typeof(CheatPanel));
            editor.titleContent = new GUIContent("Cheat Panel");
        }

        private void OnGUI() {
            EditorUtilities.DappsMenu("Cheat Panel");

            if (!Application.isPlaying) {
                OfflineMode();
            } else {
                OnlineMode();
            }
        }

        private void OfflineMode() {
            PlayerPrefsSettings();
            GetSelections();
        }

        private void PlayerPrefsSettings() {
            GUILayout.Label("- Save Data -", EditorUtilities.BoldLabelStyle(Color.white));
            if (GUILayout.Button("Delete All", GUILayout.Height(25))) {
                PlayerPrefs.DeleteAll();
            }

        }

        private void GetSelections() {
            GUILayout.Label("- Selectables -", EditorUtilities.BoldLabelStyle(Color.white));
            if (GUILayout.Button("GetSelections", GUILayout.Height(25))) {
                foreach (Selectable selectableUI in Selectable.allSelectablesArray) {
                    Debug.Log(selectableUI.name, selectableUI.gameObject);
                }
            }
        }


        private void OnlineMode() {
            ChangeTimeSpeed();
            Money();
            WeaponsPreview();
        }

        private void ChangeTimeSpeed() {
            GUILayout.Label("- Time Control -", EditorUtilities.BoldLabelStyle(Color.white));
            ChangeSpeed(0, 0.01f, 0.1f, .25f, .5f, 1, 1.25f, 1.5f, 1.75f, 2f, 5f, 10f, 25f, 50f);

            void ChangeSpeed(params float[] amount) {
                GUILayout.BeginHorizontal();
                foreach (float value in amount) {
                    Button($"{value}", () => {
                        Time.timeScale = value;
                    }, amount.Length);
                }
                GUILayout.EndHorizontal();
            }
        }

        private void Money() {
			GUILayout.Label("- Money -", EditorUtilities.BoldLabelStyle(Color.white));
            ChangeSpeed(0, 1f, 5f, 10f, 100f, 1000f, 10000f);

			void ChangeSpeed(params float[] amount) {
				GUILayout.BeginHorizontal();
				foreach (float value in amount) {
					Button($"+{value}", () => {
                        Players.PlayerWallet.instance.AddMoney(value);
					}, amount.Length);
				}
				GUILayout.EndHorizontal();
			}
		}

        private void WeaponsPreview() {
			GUILayout.Label("- Weapons -", EditorUtilities.BoldLabelStyle(Color.white));
            if (Players.PlayerUpgrades.instance == null) return;
            if(Players.PlayerUpgrades.instance.GetUpgrade(UpgradePreset.UpgradeType.Power).UpgradePreset() is WeaponUpgradePreset weaponUpgradePreset) {
				for (int a = 0; a < weaponUpgradePreset.weaponProgress.Length; a++) {
                    var weapon = weaponUpgradePreset.weaponProgress[a];
					if (GUILayout.Button(weapon.name, GUILayout.Height(25))) {
                        weaponUpgradePreset.SetLevel(weapon.stats.minLevel);
                        WeaponController.instance.SetNewWeapon();
					}
				}
            }
		}


		private void Button(string title, System.Action action, float amount) {
            float width = position.width / amount;
            if (GUILayout.Button(title, GUILayout.Height(25), GUILayout.Width(width - 3.5f))) {
                action?.Invoke();
            }
        }
    }
}