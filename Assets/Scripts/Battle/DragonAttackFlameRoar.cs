using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DragonAttackFlameRoar : DragonAttackDeluxe {

	Player player;
	bool hurt_box_active = false;

	[Header("Properties")]
	[SerializeField]
	float telegraph_time = 1f;
	[SerializeField]
	float attack_duration = 2f;

	[Header("References")]
	[SerializeField]
	ParticleSystem flames;
	[SerializeField]
	SpriteRenderer sprite;

	void Start() {
		player = HushPuppy.safeFindComponent("Player", "Player") as Player;

		sprite.enabled = false;
		Set_Rotation();
	}

	void Update() {
		Set_Rotation();
	}

	void Set_Rotation() {
		Vector2 diff = this.transform.position - player.transform.position;
		float rotation = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		this.transform.rotation = Quaternion.Euler(0f, 0f, rotation - 90);
	}

	public IEnumerator Attack() {

		//telegraph
		sprite.enabled = true;
		sprite.color = HushPuppy.getColorWithOpacity(sprite.color, 0f);
		sprite.DOFade(0.5f, 0.1f);
		yield return new WaitForSeconds(telegraph_time);

		//capable of doing damage
		hurt_box_active = true;
		flames.Play();
		sprite.color = HushPuppy.getColorWithOpacity(sprite.color, 1f);
		yield return new WaitForSeconds(attack_duration);

		//end
		flames.Stop();
		sprite.color = HushPuppy.getColorWithOpacity(sprite.color, 0.5f);
		yield return new WaitForSeconds(flames.main.startLifetime.constant);

		hurt_box_active = false;
		sprite.DOFade(0f, 0.1f);
		yield return new WaitForSeconds(0.1f);
		sprite.enabled = false;
	}

	public override void OnTriggerEnter2D(Collider2D coll) {
		GameObject target = coll.gameObject;
		if (target.tag == "Player" && hurt_box_active) {
			Register_Hit(target);
		}
	}
}