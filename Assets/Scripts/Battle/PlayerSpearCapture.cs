using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpearCapture : MonoBehaviour {

	public bool can_capture = false;
	public float capture_cooldown = 2f;
	public SpriteRenderer sprite;

	void OnTriggerStay2D(Collider2D coll) {
		GameObject target = coll.gameObject;
		if (target.tag == "Spear" && can_capture) {
			WeaponSpear spear = target.GetComponentInChildren<WeaponSpear>();
			spear.Destroy_Now ();
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
	}
}
