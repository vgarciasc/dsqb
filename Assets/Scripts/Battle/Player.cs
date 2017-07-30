using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Weapon { BOW };

public class Player : MonoBehaviour {

	//references
	SpriteRenderer sr;
	Rigidbody2D rb;

	float charge_time = 1f; //time in seconds to complete a charge cycle
	bool is_charging_shot = false;

	//properties
	public Weapon current_weapon = Weapon.BOW;
	public float charge = 0f;
	public float health = 1f;
	public float stamina = 1f;
	public float invincibilityCooldown = 1f;

	[Header("Physics Properties")]
	[SerializeField]
	float speed;

	[Header("Prefab References")]
	[SerializeField]
	GameObject arrowPrefab;

	#region Start
		void Start() {
			Initialize_References();
		}

		void Initialize_References() {
			rb = this.GetComponentInChildren<Rigidbody2D>();
			sr = this.GetComponentInChildren<SpriteRenderer>();
		}
	#endregion

	#region Update and Handlers
		void Update () {
			Handle_Movement();
			Handle_Stamina();
			Handle_Weapon();
		}

		void Handle_Movement() {
			float horizontal = Input.GetAxis("Horizontal");
			float vertical = Input.GetAxis("Vertical");

			float speed = this.speed;

			if (is_charging_shot) {
				speed /= 4f; //if player is charging shot, he gets slower
			}

			rb.velocity = new Vector2(horizontal, vertical) * speed;
		}

		void Handle_Weapon() {
			switch (current_weapon) {
				case Weapon.BOW:
					Handle_Weapon_Bow();
					break;
			}
		}
	#endregion

	#region Sprite
		void Set_Alpha(float alpha) {
			if (alpha < 0f || alpha > 1f) {
				print("Alpha not received correctly.");
			}
	
			sr.color = HushPuppy.getColorWithOpacity(sr.color, alpha);			
		}
	#endregion

	#region Weapons
		#region Charge
			void Add_Charge() {
				charge += Time.deltaTime / charge_time;
				charge = Mathf.Clamp(charge, 0f, 1f);
			}

			void Reset_Charge() {
				charge = 0f;
			}
		#endregion

		void Handle_Weapon_Bow() {
			if (Input.GetButton("Fire1")) {
				Start_Bow();
			}

			if (Input.GetButtonUp("Fire1")) {
				Release_Bow(charge);
			}
		}

		void Start_Bow() {
			is_charging_shot = true;
			Stop_Stamina_Recovery();
			Add_Charge();
		}

		void Release_Bow(float charge) {
			is_charging_shot = false;
			Reset_Charge();
			Start_Stamina_Recovery();

			float minimumChargeForBow = 0.33f;
			if (charge < minimumChargeForBow) {
				return;	
			}
			
			GameObject arrow = Instantiate(
				arrowPrefab,
				this.transform.position,
				Quaternion.identity
			);

			float arrowSpeed = 50 * charge;
			arrow.GetComponentInChildren<Rigidbody2D>().AddForce(
				Vector2.up * arrowSpeed,
				ForceMode2D.Impulse
			);
		}
	#endregion

	#region Stamina
		//stamina variables
		float bow_stamina_depletion = 2f;
		Coroutine stamina_recovery = null;
		float stamina_recovery_time = 4f;

		void Handle_Stamina() {
			if (is_charging_shot) {
				stamina -= Time.deltaTime / bow_stamina_depletion; 
			}

			stamina = Mathf.Clamp(stamina, 0f, 1f);

			if (stamina == 0f && is_charging_shot) {
				Release_Bow(charge);
			}
		}

		void Start_Stamina_Recovery() {
			Stop_Stamina_Recovery();
			stamina_recovery = StartCoroutine(Recover_Stamina());
		}

		void Stop_Stamina_Recovery() {
			if (stamina_recovery != null) {
				StopCoroutine(stamina_recovery);
				stamina_recovery = null;
			}
		}

		IEnumerator Recover_Stamina() {
			yield return new WaitForSeconds(0.5f);

			while (stamina < 1f) {
				stamina += Time.deltaTime / stamina_recovery_time;
				yield return new WaitForEndOfFrame();
			}

			stamina_recovery = null;
		}
	#endregion

	#region Health
		bool took_hit_invincible = false;

		public void Take_Damage(int amount) {
			if (took_hit_invincible) {
				return;
			}
			
			health -= (float) amount / 100f;
			if (health < 0) {
				print("Player is dead!");
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
