using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {
	public static MusicManager instance;
	AudioSource audioSource; //Player
	public AudioClip startMenueMusicClip, playGameMusicClip, playScoreMusicClip, dyingClip; //konkretes Audiolog

	// Awake wird vor Start aufgerufen und ist das erste was aufgerufen wird, wenn die Klasse ein Objekt erzeugt.
	void Awake () {
		instance = this;
		audioSource = gameObject.AddComponent<AudioSource> ();

		//Sorgt dafür, das der Audioclip in einer Schleife abgespielt wird.
		audioSource.loop = true;

		// Setzt die Lautstärte auf die höchste Einstellung.
		audioSource.volume = 0.3f;
	}

	/*
	 * Spielt den MenueMusicaudioclip ab.
	*/
	public void playStartMenueMusic() {

		audioSource.Stop ();
		audioSource.clip = startMenueMusicClip;
		audioSource.Play ();

	}

	/*
	 * Spielt den GameMusicaudioclip ab.
	*/
	public void playGameMusic() {

		audioSource.Stop ();
		audioSource.clip = playGameMusicClip;
		audioSource.Play ();

	}

	/*
	 * Spielt den ScoreMusicaudioclip ab.
	*/
	public void playScoreMenueMusic() {

		audioSource.Stop ();
		audioSource.clip = playScoreMusicClip;
		audioSource.Play ();

	}

	public void dyingSound() {

		audioSource.Stop ();
		audioSource.clip = dyingClip;
		audioSource.Play ();

	}
}
