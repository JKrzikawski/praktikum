using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour {

    public GameObject UI_Steuerung;
    public GameObject UI_inSpielSteuerung;
	public GameObject Spiel;
    public TextMeshProUGUI UserScoreAllgemein, pointsAfterGame;

	public static GameManager instance;
	public float maxTime = 120.0f;
	private bool start = false;
	public int live = 3;
	private bool gameHasEnded = false;

	int points = 0;

	/*
	 * Das nur eine Instanz dieses GameManagers für das ganze
	 * Level existiert. 
	*/
	void Awake()
	{
		instance = this;
	}

	/*
	 * Gibt die Points heraus.
	 * Zum Beispiel für die Anzeige an der GUI.
	*/
	public int getPoints ()
	{
		return points;
	}

	/*
	 * Gibt die noch verbleibende Spielzeit in dem Level heraus.
	 * Zum Beispiel für die Anzeige an der GUI.
	*/
	public float getTimeLeft()
	{
		return maxTime;
	}

	/*
	 * Gibt das Bool start zurück, damit das GameObjekt Spiel
	 * angeschaltet wird und das Spiel beginnt.
	*/
	public bool isStarted() {
		return start;
	}

	/*
	 * Gibt die Leben an der Schnittstelle aus, damit sie von der UI ausgelesen werden können
	 * und die Lebensanzeige geupdatet werden kann. 
	*/
	public int getLive()
	{
		return live;
	}

	/*
	 * Zählt die Punkte bei jedem Hit des Charakters und eines
	 * Birds nach oben.
	*/
	public void addPoints (int amount)
	{
		points += amount; 
	}

	/*
	 * Startet den Timer.
	*/
	void startTimer ()
	{
		start = true;
	}

	/*
	 * Diese Funktion startet das Spiel. In dem die Zeit in annähernd realer Zeit
	 * abgespielt wird.
	*/
	public void startGame ()
	{
		
        UI_inSpielSteuerung.SetActive(true);
		if (Spiel != null)
			Spiel.SetActive (true);
		startTimer ();
		gameHasEnded = false;
		MusicManager.instance.playGameMusic();
	}

	/*
	 * Wird beim Start des GameManagers aufgerufen und setzt das GameObjekt Spiel auf aus, damit
	 * das Spiel pausiert bleibt bis die Funktion startGame, das Spiel startet.
	 * Dies ist nötig, damit nicht schon Gegener gespawnt werden, wenn sich der Player noch nicht
	 * bewegen kann.
	*/
	void Start()
	{
		points = 0;
		maxTime = 120.0f;
		start = false;
		live = 3;
		gameHasEnded = false;
		if (Spiel != null)
			Spiel.SetActive (false);
		//Spielt die startMenueMusic ab. 
		MusicManager.instance.playStartMenueMusic ();

	}

	/*
	 * Setzt die Szene Spiel zurück.
	*/
	public void hardReset()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene (0);
	}

	/*
	 * Setzt nur die Variablen zurück.
	*/
	public void softReset()
	{
		Start();
	}

	/*
	 * Die Funktion beendet das Spiel, indem sie das GameObjekt Spiel ausschaltet, wenn
	 * die Leben aufgebraucht sind oder die Zeit abgelaufen ist.
	*/
	void gameEnds ()
	{
		if (!gameHasEnded) 
		{
			MusicManager.instance.dyingSound ();
			if (Spiel != null) {
				Spiel.SetActive (false);
			}
			start = false;
			UI_inSpielSteuerung.SetActive (false);
			UI_Steuerung.SetActive (true);
			Startseitenskript.instance.oeffneGameOverSeite ();
			UserScoreAllgemein.SetText ("erreichte Punkte: " + points);
            pointsAfterGame.SetText("Deine Punkte: " + points);
            gameHasEnded = true;

            MusicManager.instance.dyingSound();
			//Schreibt die Score in die Datenbank
			Datenbankverwaltung.instance.writeScore (points);
		}


	}
	/*
	 * Zählt die Zeit herunter bei jedem Frame.
	 * Aber nur wenn die Methode startTime gestartet wurde.
	*/
	void Update()
	{
		if(start && !gameHasEnded)
		{
			if (maxTime > 0.0f) { 
				maxTime -= Time.deltaTime;
			} else 
			{
				gameEnds ();
			}
		}
			
	}

	/*
	 * Lebensverwaltung. Hier werden die Leben, die der
	 * Charakter hat, bei einem Hit heruntergezählt.
	*/
	public void liveLost()
	{
		live -= 1;
		if(live <= 0)
		{
			gameEnds ();
		}
	}
		

}
