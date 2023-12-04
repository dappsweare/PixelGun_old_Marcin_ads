using Level;
using TMPro;
using UnityEngine;

namespace World {
	public class MultiplierBox : MonoBehaviour {

		private enum MultiplierType {
			Add,
			Subtract,
			Multiply,
			Divide
		}

		private readonly string[] multiplierText = new string[] {
		"+",
		"-",
		"x",
		"/",
	};

		[Header("Components")]
		[SerializeField] private TMP_Text boxText = null;
		[SerializeField] private MeshRenderer meshRenderer = null;

		[Header("Colors")]
		[SerializeField] private Color boxColor = Color.white;
		[SerializeField] private Color textColor = Color.white;
		[SerializeField] private Color coinColor = Color.white;

		[Header("Settings")]
		[SerializeField] private MultiplierType multiplierType = MultiplierType.Add;
		[SerializeField] private float multiplierValue = 1f;
		[SerializeField] private Vector3 coinScale = Vector3.one * 3;
		[SerializeField] private bool createFloatingText = false;

		private void Start() {
			ModifyColor(boxColor);
			boxText.color = textColor;
			boxText.text = string.Format("{0}{1}", multiplierText[(int)multiplierType], multiplierValue);
		}

		private void OnTriggerEnter(Collider other) {
			if (other.gameObject.TryGetComponent(out WorldPixel worldPixel)) {
				var upgradeIncome = Players.PlayerUpgrades.instance.GetUpgrade(Upgrades.UpgradePreset.UpgradeType.Income).UpgradePreset().GetValue();
				switch (multiplierType) {
					case MultiplierType.Add: upgradeIncome += multiplierValue; break;
					case MultiplierType.Subtract: upgradeIncome -= multiplierValue; break;
					case MultiplierType.Multiply: upgradeIncome *= multiplierValue; break;
					case MultiplierType.Divide: upgradeIncome /= multiplierValue; break;
				}

				float final = (float)System.Math.Round(upgradeIncome, 2);
				Players.PlayerWallet.instance.AddMoney(final);
				if (createFloatingText)
					UI.FloatingTextManager.instance.CreateText(worldPixel.transform.position, $"+ {final}", boxText.color);
				BoxManager.instance.CreateCoin(worldPixel.transform, final, coinColor, worldPixel.GetComponent<Rigidbody>().velocity, coinScale);
				Destroy(worldPixel.gameObject);
			}
		}

		private void ModifyColor(Color color) {
			var materials = meshRenderer.materials;
			for (int a = 0; a < materials.Length; a++) {
				materials[a].color = color;
			}
			meshRenderer.materials = materials;
		}
	}
}