using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonAttackPhysical : DragonAttackDeluxe {
	public bool hurt_active = false;
	public bool player_in_view = false;

	public void OnTriggerStay2D(Collider2D coll) {
		GameObject target = coll.gameObject;
		if (target.tag == "Player") {
			if (hurt_active) {
				target.GetComponentInChildren<Player>().Take_Damage(damage);
				Register_Hit();
			}
		}
	}

	public override void OnTriggerEnter2D(Collider2D coll) {
		GameObject target = coll.gameObject;
		if (target.tag == "Player") {
			player_in_view = true;
		}
	}

	public void OnTriggerExit2D(Collider2D coll) {
		GameObject target = coll.gameObject;
		if (target.tag == "Player") {
			player_in_view = false;
		}
	}

	public override void Register_Hit() {

	}

	public void Set_Active(bool value) {
		hurt_active = value;
		this.GetComponentInChildren<SpriteRenderer>().enabled = value;
	}
}
