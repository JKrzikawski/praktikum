using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

/*
 * Dieses Skript steuert die Funktionalität der UI, 
 * die während des Spielens des Levels angezeigt wird. 
 * --> Scoreanzeige, Zeitanzeige, Lebensanzeige, Pausemenü
 */
public class UIimSpiel_Skript : MonoBehaviour
{
    // Die Anziegen, die die ablaufende Zeit und den hochzählenden Score im Spiel anzeigen
    public TextMeshProUGUI timer, scoreAnzeige;

    // Die Lebensanzeigen
    public GameObject leben1, leben2, leben3;

    //Die Anzeige während das Spiel pausiert
    public GameObject pauseCanvas, uiImSpielKomplett;

    // Der Startbildschirm, damit beim Beenden des Levels die Startseite angeziegt werden kann
	public GameObject startbildschirmCanvas;

    // Die GameObjects, die das Hauptmenü und die UI im Spiel steuern
	public GameObject UI_Steuerung, UI_inSpielSteuerung;

    // Ein Boolean, der angibt, ob das Spiel gerade pausiert ist (das Pause-Fenster offen ist)
	private bool pause = false;


    /*
    *gibt einen Boolean zurück, ob das Spiel gerade pausiert ist
    */
	public bool getpause()
	{
		return pause;
	}

    //schließt die UI im Spiel (wichtig bei Spielende)
    // und setzt auch die Pauseseite inactive
    public void schließeUIAnzeigen()
    {
        uiImSpielKomplett.SetActive(false);
		pauseCanvas.SetActive (false);
    }

    // Setzt die UI im Spiel active --> sichtbar
    public void starteUIImSpiel()
    {
        uiImSpielKomplett.SetActive(true);
        pauseCanvas.SetActive(false);
    }

    /*
    *Update is called once per frame
    *Aktualisiert die Zeit-, Score- und Lebensanzeige
    *"Wartet" auf die Aktivierung der Pauseseite durch einen Druck auf die Escape-Taste
    */
        private void Update()
    {       // Die Zeit- und Score-Anzeige
			timer.text = "Zeit: " + ((int)(GameManager.instance.getTimeLeft () + 0.9f));
			scoreAnzeige.text = "Score: " + GameManager.instance.getPoints ();

      // Die verbleibenden Leben werden angezeigt
		zeigeLebenAn(GameManager.instance.getLive());

     //wenn Escape-Button gedrückt wird, dann wird PauseCanvas visible
		if ((Input.GetKeyDown (KeyCode.Escape)) && pause == false) 
		{
            // Pauseseite sichtbar
			pauseCanvas.SetActive (true);
            // Spiel ist pausiert
			pause = true;
            // Zeit auf 0 setzen
			Time.timeScale = 0.0f;
		} 
        // Spiel wird fortgesetzt, wenn Escape gedrückt wird, während Pause ist
		else if((Input.GetKeyDown (KeyCode.Escape)) && pause == true)
		{
			spielFortsetzen ();
		}
    }

    /*
    *Fortsetzen des Spieles
   */
        public void spielFortsetzen()
	{	// deaktivieren der Pauseseite
		pauseCanvas.SetActive(false);	
        // Es ist keine Pause mehr
		pause = false;
        // Zeit auf nirmal setzen
		Time.timeScale = 1.0f;
	}

    /*
    *Die Startseite wird wieder angezeigt
    */ 
    public void zurueckZumStartbildschrim()
	{
        // UI im Spiel deaktivieren
		UI_inSpielSteuerung.SetActive (false);
        pauseCanvas.SetActive(false);

        // Startseite anzeigen
        UI_Steuerung.SetActive (true);
		startbildschirmCanvas.SetActive (true);

        // Das Spiel wird zurückgesetzt
		GameManager.instance.softReset ();
		Time.timeScale = 1.0f;
        pause = false;
    }

    /*
     * Lebensanzeige im Spiel
    *Zeigt x (anzahlLeben) in der UI im Spiel an
    */
    void zeigeLebenAn(int anzahlLeben)
    {
        if (anzahlLeben == 3)
        {
            leben1.gameObject.SetActive(true);
            leben2.gameObject.SetActive(true);
            leben3.gameObject.SetActive(true);
        }
        else if (anzahlLeben == 2)
        {
            leben1.gameObject.SetActive(false);
            leben2.gameObject.SetActive(true);
            leben3.gameObject.SetActive(true);
        }
        else if (anzahlLeben == 1)
        {
            leben1.gameObject.SetActive(false);
            leben2.gameObject.SetActive(false);
            leben3.gameObject.SetActive(true);

        }
        else
        {
            leben1.gameObject.SetActive(false);
            leben2.gameObject.SetActive(false);
            leben3.gameObject.SetActive(false);
        }
    }
}
    
