using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanFixArrow : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D coll) {
		GameObject target = coll.gameObject;
		if (target.tag == "Arrow") {
			target.GetComponentInChildren<WeaponArrow>().Fix_Arrow_To(this.gameObject);
		}
	}
}