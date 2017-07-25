using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour {

	[Header("General References")]
	[SerializeField]
	Player player;

	[Header("HUD References")]
	[SerializeField]
	Image health_indicator;
	[SerializeField]
	Image stamina_indicator;
	[SerializeField]
	Image weapon_indicator;

	[Header("Sprites")]
	[SerializeField]
	List<Sprite> bow_sprites = new List<Sprite>();

	void Update() {
		Update_Health();
		Update_Stamina();
		Update_Weapon();
	}

	void Update_Health() {
		health_indicator.fillAmount = player.health;
	}

	void Update_Stamina() {
		stamina_indicator.fillAmount = player.stamina;
	}

	void Update_Weapon() {
		switch (player.current_weapon) {
			case Weapon.BOW:
				Update_Bow();
				break;
		}
	}

	void Update_Bow() {
		float unit = 1f / bow_sprites.Count;

		for (int i = 1; i < bow_sprites.Count; i++) {
			if (player.charge > unit * (i-1) &&
				player.charge < unit * (i)) {
				weapon_indicator.sprite = bow_sprites[i];
			}
		}

		if (player.charge <= 0.01f) {
			weapon_indicator.sprite = bow_sprites[0];
		}
	}
}
