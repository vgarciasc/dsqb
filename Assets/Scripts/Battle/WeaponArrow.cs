using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponArrow : WeaponDeluxe {
	
	void Update () {
		Handle_Direction();
	}

	public override void Register_Hit(GameObject target) {
		target.GetComponentInChildren<Dragon>().Take_Damage_Arrow(this);
		
		if (destroy_on_contact) {
			Destroy(this.gameObject);
			return;
		}
	}

	public void Fix_Arrow_To(GameObject target) {
		sr.transform.parent = target.transform;
		sr.transform.SetAsLastSibling();
		sr.sortingOrder = 0;

		Destroy(this.gameObject);
	}
}
