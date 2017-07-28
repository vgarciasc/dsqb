using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : MonoBehaviour {

	SpriteRenderer sr;
	Rigidbody2D rb;
	Vector3 current_target = Vector2.zero;
	float speed = 3;
	Player player;

	[Header("Dragon Properties")]
	public float health = 1f;
	public float invincibilityCooldown = 2f;
	
	[Header("Dragon Attacks")]
	[SerializeField]
	GameObject dragonAttack_fireballPrefab;
	[SerializeField]
	GameObject dragonAttack_physical;	

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
		if (Input.GetKeyDown(KeyCode.E)) {
			Attack_Throw_Fireball();
		}
		if (Input.GetKeyDown(KeyCode.F)) {
			StartCoroutine(Attack_Physical());
		}

		Handle_Movement();
	}

	void Handle_Movement() {
		rb.velocity = (current_target - transform.position).normalized * speed;
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
	#endregion

	#region sprite
		void Set_Alpha(float alpha) {
			if (alpha < 0f || alpha > 1f) {
				print("Alpha not received correctly.");
			}
	
			sr.color = HushPuppy.getColorWithOpacity(sr.color, alpha);			
		}
	#endregion

	#region health
		bool took_hit_invincible = false;

		public void Take_Damage(int amount) {
			if (took_hit_invincible) {
				return;
			}

			health -= (float) amount / 100f;
			if (health < 0) {
				print("Dragon is dead!");
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

	#region attacks
		public void Attack_Throw_Fireball() {
			GameObject fireball = Instantiate(
				dragonAttack_fireballPrefab,
				this.transform.position,
				Quaternion.identity
			);

			var fb_rb = fireball.GetComponentInChildren<Rigidbody2D>();
			fb_rb.velocity = (player.transform.position - fb_rb.transform.position).normalized * 10;
			fb_rb.angularVelocity = 360;
		}

		public IEnumerator Attack_Physical() {
			dragonAttack_physical.SetActive(true);
			yield return new WaitForSeconds(1f);
			dragonAttack_physical.SetActive(false);
		}
	#endregion
}