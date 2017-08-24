using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdScript: MonoBehaviour {
	bool hit = false;
	private Animator _anim;
	private Collider2D[] _colliders;
	private AudioSource source;
	System.Random rnd;

	// Use this for initialization
	void Start () {
		_anim = GetComponent<Animator>();
		_colliders = GetComponents<Collider2D> ();
		source = GetComponent<AudioSource>();
		rnd = new System.Random ();
	}
	
	/*Update is called once per frame
	* Es vernichtet die geklonten GameObjekts vom Bird,
	* wenn das Boolean hit true wird.
	*/
	void Update () {
		if(hit)
			DestroyImmediate (gameObject);
	}

	/*
	 * Registiert Trigger und setzt die hit Variable
	 * auf true, wenn die Killzone oder der Player
	 * den Trigger ausgelöst hat.
	*/
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag.Equals("Killzone")) 
		{
			hit = true;
		}
		else if (other.gameObject.tag.Equals("Player")) 
		{
			hit = true;
		}
	}

	/*
	 * Registiert die Kollison mit dem Boden und schmeißt
	 * dann den Collider heraus, damit die Vögel durch den Boden fallen
	 * und auf die Killzone treffen können.
	 * Setzt die Animation collision und spielt den Sound Parrot Call #2 ab.
	*/
	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag.Equals("Boden")) 
		{	
			float pitch = rnd.Next (100) * 0.01f;
			pitch *= 0.5f;
			pitch -= 0.25f;
			pitch += 1.0f;
			source.pitch = pitch;
			source.Play ();
			_anim.SetBool ("collision", true);
			for (int i = 0; i < _colliders.Length; i++) {
				if (!_colliders [i].isTrigger)
					Destroy (_colliders [i]);
			}

		}
	}
}
