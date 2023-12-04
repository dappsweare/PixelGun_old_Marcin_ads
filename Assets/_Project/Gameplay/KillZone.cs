using DG.Tweening.Core.Easing;
using Level;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons;

public class KillZone : MonoBehaviour
{
	private void OnCollisionEnter(Collision collision) {
		if (collision.collider.TryGetComponent(out LevelPixel levelPixel)) {
			levelPixel.Death();
		}
	}
}
