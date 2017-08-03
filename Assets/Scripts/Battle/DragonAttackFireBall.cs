using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonAttackFireBall : DragonAttackDeluxe {
	[SerializeField]
	GameObject fireball;
	[SerializeField]
	ParticleSystem rock_destroy;

	bool can_destroy = false;

	void Start() {
		StartCoroutine (Destroy_Cooldown ());
	}

	IEnumerator Destroy_Cooldown() {
		yield return new WaitForSeconds (0.5f);
		can_destroy = true;
	}

	void OnTriggerEnter2D(Collider2D coll) {
		GameObject target = coll.gameObject;
		if (target.tag == "Spear") {
			WeaponSpear spear = target.GetComponentInChildren <WeaponSpear> ();
			if (Is_Harmful_Spear (spear)) {
				spear.Stop ();
				Divide_Asteroid ();
			}
		}
	}

	void OnTriggerStay2D(Collider2D coll) {
		GameObject target = coll.gameObject;
		if (target.tag == "Spear") {
			WeaponSpear spear = target.GetComponentInChildren <WeaponSpear> ();
			if (Is_Harmful_Spear (spear)) {
				spear.Stop ();
				Divide_Asteroid ();
			}
		}
	}

	bool Is_Harmful_Spear(WeaponSpear spear) {
		return (can_destroy && spear.GetComponentInChildren<Rigidbody2D> ().velocity.magnitude > 0.25f);
	}

	void Divide_Asteroid() {
		GameObject go;
		Rigidbody2D fb_rb;
		Vector3 velocity = GetComponentInChildren <Rigidbody2D> ().velocity;

		go = Instantiate (fireball, this.transform.position, Quaternion.identity);
		fb_rb = go.GetComponentInChildren <Rigidbody2D>();
		go.GetComponentInChildren<DragonAttackFireBall>().rock_destroy.Play();
		fb_rb.velocity = Quaternion.Euler (new Vector3 (0f, 0f, 30f)) * velocity;
		go.transform.localScale *= 0.75f;

		go = Instantiate (fireball, this.transform.position, Quaternion.identity);
		fb_rb = go.GetComponentInChildren <Rigidbody2D>();
		go.GetComponentInChildren<DragonAttackFireBall>().rock_destroy.Play();
		fb_rb.velocity = Quaternion.Euler (new Vector3 (0f, 0f, -30f)) * velocity;
		go.transform.localScale *= 0.75f;

		Destroy (this.gameObject);
	}
}