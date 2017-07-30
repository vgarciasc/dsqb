using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponArrow : WeaponDeluxe {

	Rigidbody2D rb;

	[SerializeField]
	SpriteRenderer sr;

	void Start () {
		rb = this.GetComponentInChildren<Rigidbody2D>();
	}
	
	void Update () {
		Handle_Direction();
	}

	void Handle_Direction() {
		if (rb.velocity == Vector2.zero) {
			sr.enabled = false;
			return;
		}
		else if (!sr.enabled) {
			sr.enabled = true;
		}

		float rotation = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
		this.transform.rotation = Quaternion.Euler(0f, 0f, rotation - 90);
	}

	public override void Register_Hit(GameObject target) {
		target.GetComponentInChildren<Dragon>().Take_Damage_Arrow(this);
		
		if (destroy_on_contact) {
			Destroy(this.gameObject);
			return;
		}
	}

	public void Fix_Arrow_To(GameObject target) {
		sr.transform.parent = target.transform;
		sr.transform.SetAsLastSibling();
		sr.sortingOrder = 0;

		Destroy(this.gameObject);
	}
}
