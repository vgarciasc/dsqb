using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DragonAttackPhysical : DragonAttackDeluxe {
	[SerializeField]
	SpriteRenderer sprite;
	[SerializeField]
	float telegraph_duration;
	[SerializeField]
	float attack_duration;

	public float stun_duration = 2f;
	public bool hurt_active = false;
	public bool player_in_view = false;

	public void OnTriggerStay2D(Collider2D coll) {
		GameObject target = coll.gameObject;
		if (target.tag == "Player") {
			if (hurt_active) {
				Register_Hit(target);
			}
		}
	}

	public override void OnTriggerEnter2D(Collider2D coll) {
		GameObject target = coll.gameObject;
		if (target.tag == "Player") {
			player_in_view = true;
		}
	}

	public void OnTriggerExit2D(Collider2D coll) {
		GameObject target = coll.gameObject;
		if (target.tag == "Player") {
			player_in_view = false;
		}
	}

	public override void Register_Hit(GameObject target) {
		target.GetComponentInChildren<Player>().Take_Damage(damage);
	}

	public IEnumerator Telegraph() {
		sprite.enabled = true;

		sprite.color = HushPuppy.getColorWithOpacity(sprite.color, 0f);
		sprite.DOFade(0.3f, 0.1f);
		yield return new WaitForSeconds(telegraph_duration);
	}

	public IEnumerator Start_Attack() {
		hurt_active = true;

		sprite.color = HushPuppy.getColorWithOpacity(sprite.color, 1f);
		yield return new WaitForSeconds(attack_duration);
	}
	
	public IEnumerator End_Attack() {
		hurt_active = false;

		sprite.color = HushPuppy.getColorWithOpacity(sprite.color, 0.5f);
		sprite.DOFade(0f, 0.1f);
		
		yield return new WaitForSeconds(0.1f);
		sprite.enabled = false;
	}
}
