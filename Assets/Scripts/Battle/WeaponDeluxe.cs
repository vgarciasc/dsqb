using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDeluxe : MonoBehaviour {
	public int damage = 30;

	void OnTriggerEnter2D(Collider2D coll) {
		GameObject target = coll.gameObject;
		if (target.tag == "Dragon") {
			target.GetComponentInChildren<Dragon>().Take_Damage(damage);
			Register_Hit();
		}
	}

	void Register_Hit() {
		Destroy(this.gameObject);
	}
}
