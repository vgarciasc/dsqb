using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DragonUIManager : MonoBehaviour {
	[SerializeField]
	Dragon dragon;
	[SerializeField]
	Image health_indicator;

	void Update() {
		Update_Health();
	}

	void Update_Health() {
		health_indicator.fillAmount = dragon.health;
	}
}
