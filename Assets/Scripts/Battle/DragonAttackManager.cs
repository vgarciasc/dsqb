using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonAttackManager : MonoBehaviour {

	[SerializeField]
	Dragon dragon;

	Player player;

	[Header("Dragon Attacks")]
	[SerializeField]
	GameObject dragonAttack_fireballPrefab;
	[SerializeField]
	GameObject dragonAttack_flameTowerPrefab;
	[SerializeField]
	DragonAttackPhysical dragonAttack_physical;	
	[SerializeField]
	DragonAttackFlameRoar dragonAttack_flameRoar;
	[SerializeField]
	ParticleSystem dragonAttack_flameRoar_particleSystem;
	[SerializeField]
	float interval_between_attacks = 2f;

	void Start() {
		player = HushPuppy.safeFindComponent("Player", "Player") as Player;

		StartCoroutine(Handle_AI());
	}

	IEnumerator Handle_AI() {
		while (true) {
			yield return new WaitForSeconds(interval_between_attacks);

			float distance_between = Vector2.Distance(
				this.transform.position,
				player.transform.position
			);
			
			if (dragonAttack_physical.player_in_view) {
				yield return Attack_Physical();
				continue; //comment to view behaviour
			}

			else if (distance_between > 5f) {
				int dice = Random.Range(0, 20);
				if (dice < 10) {
					Attack_Flame_Towers(Random.Range(8, 14));
				}
				else {
					int quantity = 1;
					if (dragon.health < 0.5f) quantity = 2;
					if (dragon.health < 0.25f) quantity = 3;
					if (dragon.health < 0.1f) quantity = 5;

					yield return Attack_Throw_Fireball(quantity);
				}
			}

			else if (distance_between > 2f) {
				yield return Attack_Flame_Roar();
				continue; //comment to view behaviour
			}
		}
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.E)) {
			Attack_Throw_Fireball();
		}
		if (Input.GetKeyDown(KeyCode.F)) {
			StartCoroutine(Attack_Physical());
		}
		if (Input.GetKeyDown(KeyCode.G)) {
			Attack_Flame_Towers(8);
		}
		if (Input.GetKeyDown(KeyCode.H)) {
			StartCoroutine(Attack_Flame_Roar());
		}
	}

	IEnumerator Attack_Throw_Fireball(int quantity) {
		dragon.Set_Turn_Speed_Dampener(1.2f);

		for (int i = 0; i < quantity; i++) {
			Attack_Throw_Fireball();

			yield return new WaitForSeconds(1f);
		}

		dragon.Reset_Turn_Speed();
	}

	public void Attack_Throw_Fireball() {
		GameObject fireball = Instantiate(
			dragonAttack_fireballPrefab,
			this.transform.position,
			Quaternion.identity
		);

		float speedMultiplier = 10;
		if (dragon.health < 0.5f) speedMultiplier *= 2;
		else if (dragon.health < 0.2f) speedMultiplier *= 3f;

		var fb_rb = fireball.GetComponentInChildren<Rigidbody2D>();
		fb_rb.velocity = (player.transform.position - fb_rb.transform.position).normalized * speedMultiplier;
		fb_rb.angularVelocity = 360;
	}

	public IEnumerator Attack_Physical() {
		dragon.Set_Turn_Speed_Dampener(3f);

		yield return new WaitForSeconds(0.5f);

		dragonAttack_physical.Set_Active(true);

		yield return new WaitForSeconds(1f);

		dragonAttack_physical.Set_Active(false);
		dragon.Reset_Turn_Speed();
	}

	public void Attack_Flame_Towers(int quantity) {
		List<Vector2> tower_pos = new List<Vector2>();

		for (int i = 0; i < quantity; i++) {
			tower_pos.Add(Get_New_Flame_Tower_Position(tower_pos));
			if (tower_pos[i] != Vector2.zero) {
				GameObject tower = Instantiate(
					dragonAttack_flameTowerPrefab,
					tower_pos[i],
					Quaternion.identity
				);

				var aux = tower.GetComponentInChildren<DragonAttackFlameTower>();
				aux.Initialize(dragon.health);
			}
		}
	}

	#region Flametower

		Vector2 Get_New_Flame_Tower_Position(List<Vector2> positions) {
			Vector2 aux = Vector2.zero;
			int k = 0;

			do {
				aux = new Vector2(
					Random.Range(-8f, 8f),
					Random.Range(-4f, 4f)
				);

				if (k++ > 100) {
					print("No suitable places for flame tower.");
					return Vector2.zero;
				}
			} while (!Distant_From_Each_Position(positions, aux, 2f));

			return aux;
		}

		bool Distant_From_Each_Position(
			List<Vector2> positions,
			Vector2 to_test,
			float threshold) {

			for (int i = 0; i < positions.Count; i++) {
				if (Vector2.Distance(to_test, positions[i]) < threshold) {
					return false;
				}
			}

			return true;
		}
	#endregion

	public IEnumerator Attack_Flame_Roar() {
		yield return dragonAttack_flameRoar.Attack();
	}
}
