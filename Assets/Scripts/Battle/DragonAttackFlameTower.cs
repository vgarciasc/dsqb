using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonAttackFlameTower : DragonAttackDeluxe {
	[SerializeField]
	ParticleSystem explosion;
	[SerializeField]
	CircleCollider2D trigger_coll;

	public float base_buildup_time = 2f;
	float buildup_health_multiplier = 1f;

	void Start() {
		StartCoroutine(Behaviour());
	}

	public void Initialize(float health) {
		buildup_health_multiplier = health * 1f;
		//if dragon has 50% HP, flametowers explode 50% quicker.
	}

	IEnumerator Behaviour() {
		trigger_coll.enabled = false;

		float wait_time = Mathf.Clamp(
			buildup_health_multiplier * base_buildup_time,
			0.5f,
			base_buildup_time);
			
		yield return new WaitForSeconds(wait_time);
		
		explosion.Play();
		trigger_coll.enabled = true;

		yield return new WaitForSeconds(
			explosion.main.duration +
			explosion.main.startLifetime.constant);
		
		Destroy(this.gameObject);
	}
	
	public override void Register_Hit(GameObject target) {

	}
}
