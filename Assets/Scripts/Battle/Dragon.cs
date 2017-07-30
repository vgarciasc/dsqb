using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : MonoBehaviour {

	SpriteRenderer sr;
	Rigidbody2D rb;
	Vector3 current_target = Vector2.zero;
	float speed = 3;
	Player player;

	[HideInInspector]
	float turn_speed_dampener = 1f;

	[Header("Dragon Properties")]
	public float health = 1f;
	public float invincibilityCooldown = 2f;

	void Start () {
		Initialize_References();
		StartCoroutine(Walking_Behaviour());
	}

	void Initialize_References() {
		rb = this.GetComponentInChildren<Rigidbody2D>();
		sr = this.GetComponentInChildren<SpriteRenderer>();

		player = HushPuppy.safeFindComponent("Player", "Player") as Player;
	}

	void Update() {
		Handle_Movement();
		Handle_Aim();
	}

	void Handle_Movement() {
		rb.velocity = (current_target - transform.position).normalized * speed;
	}

	void Handle_Aim() {
		Vector2 diff = this.transform.position - player.transform.position;
		float rotation = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

		rotation += 90;

		Quaternion quat = Quaternion.identity;
		quat.eulerAngles = new Vector3(0f, 0f, rotation);

		// this.transform.rotation = Quaternion.Euler(0f, 0f, rotation);
		this.transform.rotation = Quaternion.RotateTowards(
			this.transform.rotation,
			quat,
			Time.deltaTime * 100f / turn_speed_dampener
		);
	}

	#region movement
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

		public void Set_Turn_Speed_Dampener(float dampener) {
			turn_speed_dampener = dampener;
		}

		public void Reset_Turn_Speed() {
			turn_speed_dampener = 1f;
		}
	#endregion

	#region sprite
		void Set_Alpha(float alpha) {
			if (alpha < 0f || alpha > 1f) {
				print("Alpha not received correctly.");
			}
	
			foreach (SpriteRenderer s in GetComponentsInChildren<SpriteRenderer>()) {
				s.color = HushPuppy.getColorWithOpacity(s.color, alpha);			
			}
		}
	#endregion

	#region health
		bool took_hit_invincible = false;

		public void Take_Damage_Arrow(WeaponArrow arrow) {
			if (took_hit_invincible) {
				return;
			}
			
			arrow.Fix_Arrow_To(this.gameObject);
			Take_Damage(arrow.damage);
		}

		public void Take_Damage(int amount) {
			if (took_hit_invincible) {
				return;
			}

			health -= (float) amount / 100f;
			if (health < 0) {
				((SceneLoader) HushPuppy.safeFindComponent("GameController", "SceneLoader")).Victory();
				Destroy(this.gameObject);
			}

			StartCoroutine(Take_Damage_Cooldown());
		}

		IEnumerator Take_Damage_Cooldown() {
			took_hit_invincible = true;
			Set_Alpha(0.5f);

			yield return new WaitForSeconds(invincibilityCooldown);	

			took_hit_invincible = false;
			Set_Alpha(1f);
		}
	#endregion
}