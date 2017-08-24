using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody2D _myRigidbody;
    private Animator _anim;
    private SpriteRenderer _spriteRenderer;
    public float speed;
	private bool ground = true;
	AudioSource audioSource;
	public AudioClip jumpClip, hitClip, birdClip;
	System.Random rnd;
	bool Spiel = true;

	// Use this for initialization
	void Start () {
        _myRigidbody = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
		audioSource = gameObject.AddComponent<AudioSource> ();
		rnd = new System.Random ();
	}
	
	// Update is called once per frame
	void Update ()
    {
        float move = Input.GetAxisRaw("Horizontal");  //Bewegung über Pfeiltasten
        _anim.SetFloat("Speed",Mathf.Abs(move)); //gibt nur absolute Zahlen wieder

        
	//Charakternimation flipt, wenn Pfeiltasten gewechselt werden 
        if(_spriteRenderer.flipX == true && move > 0)
        {
            _spriteRenderer.flipX = false;
            
        }

   		else if (_spriteRenderer.flipX == false && move < 0)
        {
            _spriteRenderer.flipX = true;
            
        }
	}

	void FixedUpdate()
	{
		_myRigidbody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * speed * Time.fixedDeltaTime, _myRigidbody.velocity.y);

		// Fähigkeit zu springen und spielt den Sound Player-jump2 ab
		if(Input.GetKeyDown (KeyCode.Space) && ground || Input.GetKeyDown(KeyCode.UpArrow) && ground)
		{
			_myRigidbody.AddForce(new Vector2(0,18.0f),ForceMode2D.Impulse);
			float pitch = rnd.Next (100) * 0.01f;
			pitch *= 0.25f;
			pitch -= 0.125f;
			pitch += 1.0f;
			audioSource.pitch = pitch;
			audioSource.clip = jumpClip;
			audioSource.Play ();
			ground = false;
		}
			

	}
	// Beschränkung, dass nur gesprungen werden darf, wenn Charakter den Boden berührt
	void OnCollisionEnter2D(Collision2D collision)
	{
		foreach (ContactPoint2D contact in collision.contacts)
		{
			if (contact.normal.y > 0.7) 
			{
				_anim.SetBool("Jump", false);
				ground = true;
			}
		}
	}
	// Beschränkung, was passieren soll, wenn Bird oder Bombe mit Charakter kollidieren
	// Score hochzählen, wenn Bird oder AngryBird - Leben verlieren, wenn Bomb
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag.Equals ("Bird")) {
			audioSource.clip = birdClip;
			audioSource.Play ();
			GameManager.instance.addPoints (1);
		} else if (other.gameObject.tag.Equals ("AngryBird")) {
			audioSource.clip = birdClip;
			audioSource.Play ();
			GameManager.instance.addPoints (-2);
		} else if (other.gameObject.tag.Equals ("FancyBird")) {
			audioSource.clip = birdClip;
			audioSource.Play ();
			GameManager.instance.addPoints (3);
		}
		else 
		{
			_anim.SetBool ("lifeEmpty", true);
			GameManager.instance.liveLost ();
			audioSource.clip = hitClip;
			audioSource.Play ();
			if(gameObject.activeInHierarchy)
			StartCoroutine(deathAniReset ());
		}

	}

	IEnumerator deathAniReset() {
		yield return new WaitForSeconds (0.5f);
		_anim.SetBool ("lifeEmpty", false);
	}
}
