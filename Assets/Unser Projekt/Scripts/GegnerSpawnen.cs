using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GegnerSpawnen : MonoBehaviour {

	public GameObject[] Voegel;
	public GameObject Bombe;
	public Vector2 SpawnValues;
	public int StartWait;
	private float SpawnWait;
	private GameObject randomGegner;
	private float MinimumSpawnWait;
	private float MaximumSpawnWait;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		// wartet bis das Spiel beginnt
		if (!enabled)
			return;
		SpawnWait -= Time.deltaTime;
		if (SpawnWait < 0.0f) {
		/* Es wird random eine Zeit erzeugt, die vergeht bis der nächste Gegner erzeugt wird
		 * Je weiter das Spiel fortgeschritten ist, desto kleiner wird dieser Zeitraum
		 * Zu Beginn des Spiels dauert es 1.8 - 2.2 Sekunden bis zur nächsten Erzeugung
		 * Am Ende 0.3 - 0.7 Sekunden
		 */
			SpawnWait = Random.Range (MinimumSpawnWait, MaximumSpawnWait);
			MinimumSpawnWait = 0.3f + 0.0125f * GameManager.instance.getTimeLeft ();
			MaximumSpawnWait = 0.7f + 0.0125f * GameManager.instance.getTimeLeft ();

			// es wird random ausgewählt ob eine Bombe oder ein Vogel geschmissen wird
			int random = Random.Range (0, 2);
			//wählt random einen Vogel aus
			if (random == 0) {
				randomGegner = Voegel [Random.Range (0, Voegel.Length)];
			} 
			else {
				randomGegner = Bombe;
			}
			// es wird random eine Position bestimmt an der der Gegner auftaucht
			Vector2 SpawnPosition = new Vector2 (Random.Range (-SpawnValues.x, SpawnValues.x), SpawnValues.y);
			/* Der Gegner wird erzeugt
			 * mit einem random Spin
			 */
			GameObject gegner = Instantiate (randomGegner, SpawnPosition, transform.rotation);
			Rigidbody2D rb = gegner.GetComponent<Rigidbody2D> ();
			rb.AddForce (new Vector2 ((Random.Range (0, 60) * 0.1f)-3.0f, (Random.Range (0, 60) * 0.1f)-3.0f), ForceMode2D.Impulse);
			rb.AddTorque ((Random.Range (0, 60) * 0.1f) - 3.0f, ForceMode2D.Impulse);
		}
	}
}
