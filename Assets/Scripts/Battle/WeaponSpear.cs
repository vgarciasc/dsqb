using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpear : WeaponDeluxe {

	void Start () {
		Initialize();
	}

	void Update() {
		Handle_Direction();
	}

	void Initialize() {
		rb = GetComponentInChildren<Rigidbody2D>();
	}
}
