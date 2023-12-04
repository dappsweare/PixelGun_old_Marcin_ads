using UnityEngine;
using System.Collections;

public class AnimatedTexture : MonoBehaviour {
	public float fps = 30.0f;
	public Texture2D[] frames;

	private int frameIndex;
	private MeshRenderer rendererMy;
	private Coroutine frameCoroutine = null;

	void Start() {
		rendererMy = GetComponent<MeshRenderer>();
	}

	public void Play() {
		rendererMy.enabled = true;
		frameCoroutine = StartCoroutine(Frames());
	}

	public void Stop() {
		rendererMy.enabled = false;
		StopCoroutine(frameCoroutine);
	}


	private IEnumerator Frames() {
		yield return new WaitForSeconds(1 / fps);
		rendererMy.sharedMaterial.SetTexture("_MainTex", frames[frameIndex]);
		frameIndex = (frameIndex + 1) % frames.Length;

		if (frameIndex != 0)
			frameCoroutine = StartCoroutine(Frames());
		else
			Stop();
	}
}