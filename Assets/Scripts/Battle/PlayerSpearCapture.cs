using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpearCapture : MonoBehaviour {

	public bool can_capture = false;
	public float capture_cooldown = 2f;
	public Collider2D capture_collider;
	public SpriteRenderer sprite;
	public GameObject bloodSplatter;

	void OnTriggerEnter2D(Collider2D coll) {
		GameObject target = coll.gameObject;
		if (target.tag == "Spear" && can_capture) {
			var aux = target.GetComponentInChildren <WeaponSpear> ();
			if (aux == null) {
				Get_Spear(target);
			} else {
				Get_Spear(aux);
			}
		}
	}

	public void Start_Capture() {
		if (!can_capture) {
			Toggle_Capture (true);
		}
	}

	public void End_Capture() {
		if (can_capture) {
			Toggle_Capture (false);
		}
	}

	void Toggle_Capture(bool value) {
		can_capture = value;
		sprite.enabled = value;
		capture_collider.enabled = value;
	}

	void Get_Spear(GameObject spear) {
		Destroy (spear);
		GameObject.FindGameObjectWithTag ("Player").GetComponentInChildren <Player> ().current_weapons++;

		var dragon = GameObject.FindGameObjectWithTag("Dragon");

		Vector3 blood_pos = (spear.transform.position + dragon.transform.position) /2;

		var blood = Instantiate(bloodSplatter, blood_pos, Quaternion.identity);
		blood.GetComponentInChildren<Blood>().Initialize(3f);
	}

	void Get_Spear(WeaponSpear spear) {
		spear.Destroy_Now ();
	}
}
