using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonAttackFlameRoar : DragonAttackDeluxe {

	Player player;

	void Start() {
		player = HushPuppy.safeFindComponent("Player", "Player") as Player;

		Set_Rotation();
	}

	void Update () {
		Set_Rotation();
	}

	void Set_Rotation() {
		Vector2 diff = this.transform.position - player.transform.position;
		float rotation = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		this.transform.rotation = Quaternion.Euler(0f, 0f, rotation - 90);
	}
}