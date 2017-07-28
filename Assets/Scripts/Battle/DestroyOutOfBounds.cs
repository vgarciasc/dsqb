using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour {
	SpriteRenderer sr;
	
	public float lifetimeAfterDisappearing = 2f;

	void Start() {
		sr = this.GetComponentInChildren<SpriteRenderer>();
	}
	
	void Update() {
		if (!sr.isVisible && !is_destroying) {
			StartCoroutine(Destroy());
		}
	}

	bool is_destroying = false;
	IEnumerator Destroy() {
		is_destroying = true;
		yield return new WaitForSeconds(lifetimeAfterDisappearing);
		Destroy(this.gameObject);
	}
}
