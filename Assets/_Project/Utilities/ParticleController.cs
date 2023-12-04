using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour {

	public System.Action OnParticleComplete = null;

	private void OnEnable() => StartCoroutine(CheckParticle());

	private IEnumerator CheckParticle() {
		ParticleSystem ps = this.GetComponent<ParticleSystem>();

		while (true && ps != null) {
			yield return new WaitForSeconds(0.5f);
			if (!ps.IsAlive(true)) {
				OnParticleComplete?.Invoke();
				break;
			}
		}
	}

	public void ClearActions() {
		OnParticleComplete = null;
	}
}
