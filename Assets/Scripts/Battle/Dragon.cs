using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : MonoBehaviour {

	Rigidbody2D rb;
	Vector3 current_target = Vector2.zero;
	float speed = 3;

	void Start () {
		Initialize_References();
		StartCoroutine(Walking_Behaviour());
	}

	void Initialize_References() {
		rb = this.GetComponentInChildren<Rigidbody2D>();
	}

	void Update() {
		Handle_Movement();
	}

	void Handle_Movement() {
		rb.velocity = (current_target - transform.position).normalized * speed;
	}

	IEnumerator Walking_Behaviour() {
		while (true) {
			RaycastHit2D hit;
			Vector2 next_pos;

			do {
				next_pos = new Vector3(
					Random.Range(-4f, 4f),
					Random.Range(-4f, 4f)	
				);

				hit = Physics2D.Raycast(
					this.transform.position,
					next_pos,
					Vector2.Distance(
						this.transform.position,
						next_pos + (Vector2) this.transform.position)
				);
			} while (hit.collider != null);

			Move_Towards(next_pos);

			yield return new WaitUntil(() => 
				Vector2.Distance(
					this.transform.position,
					next_pos
				) < 0.2f
			);

			Reset_Velocity();

			yield return new WaitForSeconds(0.8f);
		}
	}
	
	void Move_Towards(Vector2 next_pos) {
		current_target = next_pos;
	}

	void Reset_Velocity() {
		current_target = this.transform.position;
		rb.velocity = Vector2.zero;
	}
}