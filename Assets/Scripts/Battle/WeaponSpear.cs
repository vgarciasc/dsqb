using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpear : WeaponDeluxe {

	[SerializeField]
	float lifetimeAfterDisappearing = 2f;

	void Start () {
		Initialize();
	}

	void Update() {
		if (!sr.isVisible && !is_destroying) {
			StartCoroutine (Destroy ());
		}

		Handle_Direction();
	}

	void Initialize() {
		rb = GetComponentInChildren<Rigidbody2D>();
	}

	public virtual void Handle_Direction() {
		if (rb.velocity == Vector2.zero) {
			return;
		}

		float rotation = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
		this.transform.rotation = Quaternion.Euler(0f, 0f, rotation - 90);
	}

	bool is_destroying = false;
	IEnumerator Destroy() {
		is_destroying = true;

		yield return new WaitForSeconds(lifetimeAfterDisappearing);

		Destroy_Now ();
	}

	public void Destroy_Now() {
		GameObject.FindGameObjectWithTag ("Player").GetComponentInChildren <Player>().current_spears++;
		Destroy(this.gameObject);
	}
}
