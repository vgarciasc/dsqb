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
	[SerializeField]
	TextMeshProUGUI weapon_quantity;

	[Header("Sprites")]
	[SerializeField]
	List<Sprite> bow_sprites = new List<Sprite>();
	[SerializeField]
	List<Sprite> spear_sprites = new List<Sprite>();

	void Start() {
		Initialize_Weapon();	
	}

	void Initialize_Weapon() {
		switch (player.current_weapon) {
			case Weapon.BOW:
				weapon_quantity.text = "";
				break;
		}
	}

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
			case Weapon.SPEAR:
				Update_Spear();
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

	void Update_Spear() {
		weapon_quantity.text = "x" + player.current_spears;

		float unit = 1f / spear_sprites.Count;

		for (int i = 1; i < spear_sprites.Count; i++) {
			if (player.charge > unit * (i - 1) &&
				player.charge < unit * (i)) {
				weapon_indicator.sprite = spear_sprites[i];
			}
		}

		if (player.charge <= 0.01f) {
			weapon_indicator.sprite = spear_sprites[0];
		}
	}
}
