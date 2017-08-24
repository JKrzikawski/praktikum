using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class birdfly : MonoBehaviour {

	private Rigidbody2D rb;
	private Vector2 vector;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		vector = new Vector2(-12, 0);
	}

	// Update is called once per frame
	void Update () {

		rb.AddForce(vector);

	}
}