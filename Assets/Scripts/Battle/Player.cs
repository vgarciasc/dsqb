using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	Rigidbody2D rb;
	
	[Header("Character Attributes")]
	[HideInInspector]
	public float health; //from 0 to 100
	[HideInInspector]
	public float stamina; //from 0 to 100

	[Header("Physics Properties")]
	[SerializeField]
	float speed;

	#region Start
		void Start() {
			Initialize_References();
		}

		void Initialize_References() {
			rb = this.GetComponentInChildren<Rigidbody2D>();
		}
	#endregion

	#region Update and Handlers
		void Update () {
			Handle_Input();
		}

		void Handle_Input() {
			float horizontal = Input.GetAxis("Horizontal");
			float vertical = Input.GetAxis("Vertical");

			rb.velocity = new Vector2(horizontal, vertical) * speed;
		}
	#endregion

}
