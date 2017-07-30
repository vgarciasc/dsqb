using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonAttackFlameTower : DragonAttackDeluxe {
	[SerializeField]
	ParticleSystem explosion;
	[SerializeField]
	CircleCollider2D trigger_coll;

	public float buildupTime = 2f;

	void Start() {
		StartCoroutine(Behaviour());
	}

	IEnumerator Behaviour() {
		trigger_coll.enabled = false;

		yield return new WaitForSeconds(buildupTime);
		
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
