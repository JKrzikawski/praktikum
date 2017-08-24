using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Parse;
using System.Threading.Tasks;

/*
 * Kontrolliert den Wechsel zwischen den Fenster 
 * des Hauptmenüs
 */ 
public class Startseitenskript : MonoBehaviour {

    public GameObject uiInGameKomplett, gameOverCanvas, registrierungFehler, loginFehler, creditsCanvas, steuerungsCanvas, prologCanvas, meinHighscoreCanvas, startCanvas, loginCanvas, registrationCanvas, highscoreCanvas, menueHintergrundCanvas, uiSteuerung;
	public Button loginButton, registrierenButton;

    // Felder für Login
    public InputField userEingabe, passwortEingabe;

    // Felder für Registrierung
    public InputField neuerUser, neueMail, neuesPasswort;
    public static Startseitenskript instance;
	 GameObject naechstesCanvas;

    /*
     * UI-Elemente werden unsichtbar 
     * --> wichtig für Spielstart, 
     * damit die Menüs ausgeblendet werden
     */ 
	private void invisibleAll ()
	{
		uiSteuerung.gameObject.SetActive(false);
		loginCanvas.gameObject.SetActive(false);
		highscoreCanvas.gameObject.SetActive(false);
		meinHighscoreCanvas.SetActive (false);
		startCanvas.gameObject.SetActive(false);
		registrationCanvas.gameObject.SetActive(false);
		prologCanvas.SetActive (false);
		steuerungsCanvas.SetActive (false);
		meinHighscoreCanvas.SetActive (false);
		creditsCanvas.SetActive (false);
		registrierungFehler.SetActive (false);
		loginFehler.SetActive (false);
        uiInGameKomplett.SetActive(false);
        gameOverCanvas.SetActive(false);
	}

	/*
	 * Startmethode
	 */ 
	private void Start()
    {
        instance = this;
        setzeEingabefelderZurueck();
		invisibleAll ();
        uiInGameKomplett.SetActive(true);
		uiSteuerung.gameObject.SetActive(true);
		menueHintergrundCanvas.gameObject.SetActive(true);
        loginCanvas.gameObject.SetActive(true); 
    }


    /*
	 * Methode, die den Wechsel von der Login auf die Startseite delegiert
     * Greift auf das LoginScript zu, um den Login zu starten
	 */
    public void WechselLoginAufStartseite(GameObject aktuellesCanvas)
    {
        LoginScript.instance.starteLogin(aktuellesCanvas, userEingabe.text, passwortEingabe.text);
        loginButton.interactable = false;
    }


	/*
	 * Öffnet ein Fenster mit einer Fehlermeldung auf der Login-Seite, wenn der
	 * Login von der Datenbank abgelehnt wurde
	 */
	public void oeffneLoginFehlerSeite()
	{
		loginFehler.SetActive (true);
		setzeEingabefelderZurueck();
	}

	
    	/*
	 * Methode, welche die Startseite von einer anderen Seite auf einen Button-Press hin öffnet
	 */
	public void oeffneStartseiteVon(GameObject aktuellesCanvas)
    {
        startCanvas.gameObject.SetActive(true);
        aktuellesCanvas.gameObject.SetActive(false);
        setzeEingabefelderZurueck();
    }

    /*
	 * Öffnet ein Fenster mit einer Fehlermeldung auf der Registrierungs-Seite, wenn die
	 * Registrierung von der Datenbank abgelehnt wurde
	 */
    public void oeffneRegistrierfehlerSeite()
    {
        registrierungFehler.SetActive(true);
        setzeEingabefelderZurueck();
    }

    /*
	 * Registrierungsverfahren eines Benutzers, nach erfolgreicher Registierung wird der Nutzer 
	 * auf die Hauptseite weitergeleitet
	 */
    public void registriereNutzerUndoeffneStartseite(GameObject aktuellesCanvas)
    {
        RegistrierenScript.instance.starteRegistrieren(aktuellesCanvas, neueMail.text, neuerUser.text, neuesPasswort.text);
        registrierenButton.interactable = false;
    }

        

	/*
	 * Öffnet die Loginseite von einer vorherigen Seite aus
	 */
    public void oeffneLoginseiteVon(GameObject aktuellesCanvas)
    {
		Datenbankverwaltung.instance.signOut ();
        wechselXaufY(aktuellesCanvas, loginCanvas);
        setzeEingabefelderZurueck();
    }

	/*
	 * Öffnet die Registrationsseite von einer vorherigen Seite aus
	 */
    public void oeffneRegistrationsseiteVon(GameObject aktuellesCanvas)
    {
        wechselXaufY(aktuellesCanvas, registrationCanvas);
        setzeEingabefelderZurueck();
    }

	/*
	 * Öffnet die Highscoreseite von einer vorherigen Seite aus
	 */
    public void oeffneHighscoreseiteVon(GameObject aktuellesCanvas)
    {
        wechselXaufY(aktuellesCanvas, highscoreCanvas);
        setzeEingabefelderZurueck();
    }

    /*
     * Öffnet bei Spielende die Gameoverseite
     */ 
    public void oeffneGameOverSeite()
    {
        gameOverCanvas.SetActive(true);
    }
	

    /*
     * öffnet den Highscore vom Gameoverfenster
     * Startet Highscoremusik
     */ 
    public void oeffneHighscoreVonGameover()
    {
        MusicManager.instance.playScoreMenueMusic();
        wechselXaufY(gameOverCanvas, highscoreCanvas);
    }

	/*
	 * Öffnet die Meine Highscoreseite von einer vorherigen Seite aus
	 */
	public void oeffneMeinHighscoreVon(GameObject aktuellesCanvas)
	{
        wechselXaufY(aktuellesCanvas, meinHighscoreCanvas);
	}


	/*
	 * Öffnet den Prolog von einer vorherigen Seite aus
	 */
	public void oeffnePrologVon(GameObject aktuellesCanvas)
	{
        wechselXaufY(aktuellesCanvas, prologCanvas);
	}

	/*
	 * Öffnet die Steuerungsseite von einer vorherigen Seite aus
	 */
	public void oeffneSteuerungVon(GameObject aktuellesCanvas)
	{
        wechselXaufY(aktuellesCanvas, steuerungsCanvas);
	}

	/*
	 * Öffnet die Creditsseite von einer vorherigen Seite aus
	 */
	public void oeffneCreditsVon(GameObject aktuellesCanvas)
	{
        wechselXaufY(aktuellesCanvas, creditsCanvas);
	}

	/*
	 * Startet das eigentliche Spiel
	 */
    public void starteGame()
	{
		GameManager.instance.softReset ();
		invisibleAll ();

		setzeEingabefelderZurueck();


        //Startet das Spiel 
		GameManager.instance.startGame ();
        //TODO Spielende implementieren
    }

	/*
	 * Setzt die Eingabefelder zurück
	 */
    public void setzeEingabefelderZurueck()
    {
        // Login
        userEingabe.text = "";
        passwortEingabe.text = "";

        // Registrieren
        neuerUser.text = "";
        neueMail.text = "";
        neuesPasswort.text = "";

        // Setzt Buttons wieder interctible
        loginButton.interactable = true;
        registrierenButton.interactable = true; 

    }

	/*
	 * Methode beendet das Spiel
	 */
	public void beendeSpiel()
	{
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit ();
		#endif
	}


    /*
     * Hilfsmethode, die den Wechsel von einem Fenster zum anderen macht,
     * Wird von anderen Methoden aufgerufen,
     * damit der Aufruf aus dem Spiel (Belegung von Buttons etc) einfacher ist. 
     */ 
    private void wechselXaufY(GameObject now, GameObject next)
    {
        now.SetActive(false);
        next.SetActive(true);
    }

}



