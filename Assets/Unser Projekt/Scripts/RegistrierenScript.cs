using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegistrierenScript : MonoBehaviour 
{
	public static RegistrierenScript instance;
    GameObject naechstesCanvas;

	// Use this for initialization
	void Awake ()
    {
        instance = this;
	}
	
	
    /*
     * startet den Registrierungsprozess
     * Bekommt das Fenster übergeben, 
     * von dem Prozess gestartet wurde, um es bei erflolgreicher Registrierung schließen zu können. 
     */ 
    public void starteRegistrieren(GameObject aktuellesCanvas, string neueMail, string neuerUser, string neuesPasswort)
    {
        string mail = neueMail;
        string user = neuerUser;        
        string passwort = neuesPasswort;


        Datenbankverwaltung.instance.OnRegisterFail += RegisterAbgelehnt;
        Datenbankverwaltung.instance.OnRegisterSuccess += RegisterInAkzeptiert;
        Datenbankverwaltung.instance.signUp(user, passwort, mail);

        WartenAufRegisterIn(aktuellesCanvas);
    }

    /*
	 * Methode friert die UI ein, solange auf die Antwort von der Datenbank gewartet wird
	*/
    void WartenAufRegisterIn(GameObject aktuellesCanvas)
    {
        naechstesCanvas = aktuellesCanvas;
    }



    /*
	 * Methode gibt an, was passieren soll, wenn die Registrierung akzeptiert wurde 
	 */
    void RegisterInAkzeptiert(string userName)
    {
        // Zur Startseite wechseln
        if (naechstesCanvas != null)
        {
            Startseitenskript.instance.oeffneStartseiteVon(naechstesCanvas);            
        }
    }


    /*
	 * Methode gibt an, was passeiren soll, wenn Registrierung nicht akzeptiert wurde
	 */
    void RegisterAbgelehnt(string userName, System.Exception exception)
    {
        Startseitenskript.instance.oeffneRegistrierfehlerSeite();
    }














}
