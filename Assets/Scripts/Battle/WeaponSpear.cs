using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WeaponSpear : WeaponDeluxe {

	[SerializeField]
	float lifetimeAfterDisappearing = 2f;

	bool stopped = false;
	float original_drag;

	void Start () {
		Initialize();

		StartCoroutine (Start_Stopping ());
	}

	void Update() {
		if (!sr.isVisible && !is_destroying) {
			StartCoroutine (Destroy ());
		}

		Handle_Direction();
	}

	void Initialize() {
		rb = GetComponentInChildren<Rigidbody2D>();

		original_drag = rb.drag;
		rb.drag = 0.5f;
	}

	IEnumerator Start_Stopping() {
		yield return new WaitForSeconds (0.2f);

		Stop ();
	}

	public void Stop() {
		rb.drag = original_drag;
	}

	public virtual void Handle_Direction() {
		if (rb.velocity.sqrMagnitude < 0.35f) {
			if (!stopped) {
				stopped = true;
				rb.velocity = Vector2.zero;
				transform.DOScale (this.transform.localScale - Vector3.one * 0.15f, 0.25f);
			}
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
