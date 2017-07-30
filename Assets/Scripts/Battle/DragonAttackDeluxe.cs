using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonAttackDeluxe : MonoBehaviour {
	public int damage = 30;
	public bool destroyOnHit = false;

	public virtual void OnTriggerEnter2D(Collider2D coll) {
		GameObject target = coll.gameObject;
		if (target.tag == "Player") {
			target.GetComponentInChildren<Player>().Take_Damage(damage);
			Register_Hit();
		}
	}

	public virtual void Register_Hit() {
		if (destroyOnHit) {
			Destroy(this.gameObject);
		}
	}
}
