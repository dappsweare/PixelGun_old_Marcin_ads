using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace World {
	public class BoxCoin : MonoBehaviour {

		[Header("Components")]
		[SerializeField] private MeshRenderer meshRenderer = null;
		[SerializeField] private Rigidbody rBody = null;
		[SerializeField] private TMP_Text[] texts = null;

		[Header("Settings")]
		[SerializeField] private float scaleDownDuration = 1f;
		[SerializeField] private float liveTime = 10f;
		[SerializeField] private float[] materialColorModifier = new float[]{
			0f,
			0f
		};
		[SerializeField] private float textColorModifer = .3f;

		public void Initialize(float value, Color color, Vector3 velocity, Vector3 scale, Pool.Pool<BoxCoin> pool) {

			meshRenderer.ModifyColor(color.ModifyColor(materialColorModifier[0]), color.ModifyColor(materialColorModifier[1]));

			var materials = meshRenderer.materials;
			for (int a = 0; a < materials.Length; a++) {
				materials[a].color = color.ModifyColor(materialColorModifier[a]);
			}
			meshRenderer.materials = materials;

			rBody.velocity = velocity;
			transform.localScale = scale;

			for (int a = 0; a < texts.Length; a++) {
				texts[a].color = color.ModifyColor(textColorModifer);
				texts[a].text = value.ToString();
			}

			StartCoroutine(Invoker.WaitAndInvoke(() => {
				transform.DOScale(0f, scaleDownDuration).SetEase(Ease.InOutQuart);
			}, liveTime - scaleDownDuration));

			StartCoroutine(Invoker.WaitAndInvoke(() => {
				gameObject.SetActive(false);
				pool.Release(this);
			}, liveTime));

		}
	}
}