using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript: MonoBehaviour {
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
        rnd = new System.Random();
    }
	
	// Update is called once per frame
	void Update () {
		if(hit)
			DestroyImmediate (gameObject);
	}

	// Legt fest, was bei einer Kollision mit dem Charakter passieren soll
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

	// legt fest, was bei Kollision mit dem Boden passieren soll
	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag.Equals("Boden")) 
		{

            float pitch = rnd.Next(100) * 0.01f;
            pitch *= 0.5f;
            pitch -= 0.25f;
            pitch += 1.0f;
            source.pitch = pitch;
            source.Play();
            _anim.SetBool ("collision", true);
			for (int i = 0; i < _colliders.Length; i++) {
				if (!_colliders [i].isTrigger)
					Destroy (_colliders [i]);
			}

		}
	}
}
