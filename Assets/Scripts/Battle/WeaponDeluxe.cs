using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDeluxe : MonoBehaviour {
	public int damage = 30;
	public bool destroy_on_contact = false;

	void OnTriggerEnter2D(Collider2D coll) {
		GameObject target = coll.gameObject;
		if (target.tag == "Dragon") {
			Register_Hit(target);
		}
	}

	public virtual void Register_Hit(GameObject target) {
		target.GetComponentInChildren<Dragon>().Take_Damage(damage);

		if (destroy_on_contact) {
			Destroy(this.gameObject);
		}
	}
}
