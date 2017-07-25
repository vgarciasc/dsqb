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

	void Update() {
		Update_Health();
		Update_Stamina();
	}

	void Update_Health() {
		health_indicator.fillAmount = player.health / 100;
	}

	void Update_Stamina() {
		stamina_indicator.fillAmount = player.stamina / 100;
	}

}
