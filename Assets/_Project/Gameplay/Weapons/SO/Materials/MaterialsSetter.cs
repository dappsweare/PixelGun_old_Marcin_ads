using UnityEngine;

public class MaterialsSetter : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject meshGO = null;
    [SerializeField] private Material[] materials = null;

    [Header("Settings")]
    [SerializeField] private bool getMeshRenderers = false;
	[SerializeField] private bool applyMaterials = false;

	[Header("Info")]
    [SerializeField] private MeshRenderer[] meshRenderers = null;

	private void OnValidate() {
		if(getMeshRenderers) {
			getMeshRenderers = false;
			GetMeshRenderers();
		}

		if (applyMaterials) {
			applyMaterials = false;
			ApplyMaterials();
		}
	}

	private void GetMeshRenderers() {
		meshRenderers = meshGO.GetComponentsInChildren<MeshRenderer>();
	}

	private void ApplyMaterials() {
		for (int a = 0; a < materials.Length; a++) {
			meshRenderers[a].material = materials[a];
		}
	}
}
