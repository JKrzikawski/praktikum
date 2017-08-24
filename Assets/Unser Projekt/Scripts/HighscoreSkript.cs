using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Parse;

public class HighscoreSkript : MonoBehaviour
{

    public TextMeshProUGUI highscoreTextfeldName, highscoreTextfeldPoints;
	public TextMeshProUGUI highscoreTextfeldNameAllgemein, highscoreTextfeldPointsAllgemein;
	public TextMeshProUGUI UserNamenAnzeigen;
	public TextMeshProUGUI UserNamenAllgemeinAnzeigen;
	public TextMeshProUGUI UserScore;
	public TextMeshProUGUI UserScoreAllgemein;


    // Use this for initialization
	/*
	 * Startet die Datenbankabfrage nach dem Score verzögert.
	*/
    private void Start()
    {
		StartCoroutine(verzoegerterStart());
    }

	/*
	 * Startet die Datenbankabfrage nach dem Score verzögert und aktuallisiert
	 * die Textfelder für den Usernamen und den Score auf der Highscoreseite.
	*/
	IEnumerator verzoegerterStart() {
		yield return new WaitForSeconds (0.5f);
		aktualisiereHighscoreAllgemein();
	}

    // wird bei Klick auf den Highscore-Button aufgerufen. Zeigt die Highscores an für den User. 
    public void aktualisiereHighscore()
    {
		Datenbankverwaltung.instance.OnUserTop5Received += userTop5DatenEmpfangen;
		Datenbankverwaltung.instance.OnUserTop5RequestFailed += userTop5empfangenGescheitert;
		Datenbankverwaltung.instance.getFiveBestScoreForUser ();
		UserNamenAnzeigen.SetText ( Datenbankverwaltung.instance.getCurrentUser ());
		UserScore.SetText ("erreichte Punkte: " + GameManager.instance.getPoints ());
	}

	/*
	 * Wenn die Highscoreabfrage für den User an der Datenbank gescheitert ist, gibt es
	 * eine Fehlermeldung aus.
	*/
	void userTop5empfangenGescheitert(System.Exception exception) {
		highscoreTextfeldName.SetText("Verbindung zum Server nicht möglich<br>Bitte stelle sicher,<br>dass du mit dem Internet<br>verbunden bist!");
	}

	/*
	 * Die Textfelder werden mit den Ergebnissen aus der Datenbank befüllt und zwar 
	 * auf der Mein Highscoreseite.
	*/
	void userTop5DatenEmpfangen(List<ParseObject> ergebnisse) {
		string ausgelesen = "";
		for(int i = 0; i < ergebnisse.Count; i++ )
		{
			ausgelesen += ergebnisse [i]["score"] + "<br>";
		}
		highscoreTextfeldPoints.SetText(ausgelesen);

		ausgelesen = "";
		for(int i = 0; i < ergebnisse.Count; i++ )
		{
			ausgelesen += ergebnisse [i]["playerName"] + "<br>";
		}
		highscoreTextfeldName.SetText(ausgelesen);


	}

	// wird bei Klick auf den Highscore-Button aufgerufen. Zeigt die Highscores an. 
	public void aktualisiereHighscoreAllgemein()
	{
		Datenbankverwaltung.instance.OnTop5Received += Top5DatenEmpfangen;
		Datenbankverwaltung.instance.OnTop5RequestFailed += Top5empfangenGescheitert;
		Datenbankverwaltung.instance.getFiveBestScore();
		UserNamenAllgemeinAnzeigen.SetText (Datenbankverwaltung.instance.getCurrentUser ());
		//UserScoreAllgemein.SetText ("erreichte Punkte" + GameManager.instance.getPoints ());
	}

	/*
	 * Wenn die Highscoreabfrage an der Datenbank gescheitert ist, gibt es
	 * eine Fehlermeldung aus.
	*/
	void Top5empfangenGescheitert(System.Exception exception) {
		highscoreTextfeldNameAllgemein.SetText("Verbindung zum Server nicht möglich<br>Bitte stelle sicher,<br>dass du mit dem Internet<br>verbunden bist!");
	}

	/*
	 * Die Textfelder werden mit den Ergebnissen aus der Datenbank befüllt und zwar 
	 * auf der Highscoreseite.
	*/
	void Top5DatenEmpfangen(List<ParseObject> ergebnisse) {
		string ausgelesen = "";
		for(int i = 0; i < ergebnisse.Count; i++ )
		{
			ausgelesen += ergebnisse [i]["score"] + "<br>";
		}
		highscoreTextfeldPointsAllgemein.SetText(ausgelesen);

		ausgelesen = "";
		for(int i = 0; i < ergebnisse.Count; i++ )
		{
			ausgelesen += ergebnisse [i]["playerName"] + "<br>";
		}
		highscoreTextfeldNameAllgemein.SetText(ausgelesen);


	}
		

}
