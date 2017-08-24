using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingSwanScript : MonoBehaviour {

	private Rigidbody2D rb;
	private Vector2 vector;
	private Rigidbody2D destructionPoint;
	public GameObject dP;
	public GameObject swan;
	//public static movingSwanScript instance;

	// Use this for initialization
	void Start () {
	//	instance = this;
		rb = this.GetComponent<Rigidbody2D>();
		destructionPoint = dP.GetComponent <Rigidbody2D> ();
		vector = new Vector2(-90, 0);
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (rb.position.x >= destructionPoint.position.x) {
			rb.AddForce (vector);
		} else {
			DestroyImmediate (swan);
			
		}
			
	}
}