using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginScript : MonoBehaviour
{
    public static LoginScript instance;

    GameObject naechstesCanvas;

    // Use this for initialization
    void Awake()
    {
        instance = this;

    }

    // Update is called once per frame
    void Update()
    {

    }

    /*
     * Startet Login-Prozess
     * Bekommt das Fenster übergeben, von dem der Login gestartet wurde, damit dieses bei erfolgreichem Login geschlossen wird. 
     */
    public void starteLogin(GameObject aktuellesCanvas, string user, string passwort)
    {
        string aktuellerUser = user;
        string aktuellespasswort = passwort;
        //loginButton.interactable = false;

        // Überprüft die Einlogdaten, ob sie schon in der Datenbank sind und
        //somit der Login geglückt ist (Christin)
        Datenbankverwaltung.instance.OnLoginFail += LogInAbgelehnt;
        Datenbankverwaltung.instance.OnLoginSuccess += LogInAkzeptiert;

        Datenbankverwaltung.instance.signIn(aktuellerUser, aktuellespasswort);

        WartenAufLogIn(aktuellesCanvas);
    }


    /*
	 * Methode friert die UI ein, solange auf die Antwort von der Datenbank gewartet wird
	*/
    void WartenAufLogIn(GameObject aktuellesCanvas)
    {
        naechstesCanvas = aktuellesCanvas;
        //loginButton.interactable = false;
    }


    /*
	 * Wird ausgeführt wenn der Login erfolgreich war und
	 * wenn das GameObjekt naechstesCanvas nicht Null ist.
	 * Dann leitet sie weiter auf die Startseite und setzt die
	 * Eingabefelder zurück (Anna, Christin)
	*/
    void LogInAkzeptiert(string userName)
    {
        // Zur Startseite wechseln
        if (naechstesCanvas != null)
        {
            Startseitenskript.instance.oeffneStartseiteVon(naechstesCanvas);
        }
    }

    /*
	 * Wird ausgeführt wenn der Login fehlgeschlagen hat.
	*/
    void LogInAbgelehnt(string userName, System.Exception exception)
    {
        Startseitenskript.instance.oeffneLoginFehlerSeite();
    }



























}
