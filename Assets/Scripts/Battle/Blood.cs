using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blood : MonoBehaviour {

	[SerializeField]
	SpriteRenderer sr;

	void Start () {
		Initialize(1f);
	}

	public void Initialize(float scale_base) {
		this.transform.localScale *= (Random.Range(- scale_base / 10f, scale_base / 10f) + scale_base);
		this.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 180f));

		sr.color += new Color(1f, 1f, 1f) * Random.Range(-0.1f, 0.1f);
	}
}
