using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Parse;
using System.Threading.Tasks;


public class Datenbankverwaltung : ParseInitializeBehaviour {

	public static Datenbankverwaltung instance;
	string Username1;
	string Password1;
	Task task;
	List<ParseObject> anzeige = new List<ParseObject>();


	// Da wird deklaiert wie der Delegate aufgebaut ist auch gleichzeitig eine Typdefinition. (Christin)
	public delegate void OnRegisterSuccessDelegate(string userName);  
	public delegate void OnRegisterFailDelegate(string userName, System.Exception exception); 

	public delegate void OnLoginSuccessDelegate(string userName); 
	public delegate void OnLoginFailDelegate(string userName, System.Exception exception); 

	public delegate void OnUserTop5ReceivedDelegate(List<ParseObject> results); 
	public delegate void OnUserTop5RequestFailedDelegate(System.Exception exception);

	public delegate void OnTop5ReceivedDelegate(List<ParseObject> results); 
	public delegate void OnTop5RequestFailedDelegate(System.Exception exception);

	//hier werden konkrete Variablen des Delegate angelegt. (Christin)
	public OnRegisterSuccessDelegate OnRegisterSuccess;
	public OnRegisterFailDelegate OnRegisterFail;

	public OnLoginSuccessDelegate OnLoginSuccess;
	public OnLoginFailDelegate OnLoginFail;

	public OnUserTop5ReceivedDelegate OnUserTop5Received;
	public OnUserTop5RequestFailedDelegate OnUserTop5RequestFailed;

	public OnTop5ReceivedDelegate OnTop5Received;
	public OnTop5RequestFailedDelegate OnTop5RequestFailed;

	/*
	 * Die Connetion zum Server wird aufgebaut.
	*/
	void Start () {

		connectToServer ();
		instance = this;
	}

	/*
	 * Meldet sich beim Server an.
	*/	
	void connectToServer() {
		ParseClient.Initialize(new ParseClient.Configuration {
			ApplicationId = "Ml5EHZVaSbZwTJCtrKPhDJFOhMcEE0p2kqJ85pzP",
			WindowsKey = "LFscYcc3C73XR25OYlJkXkdVG6SR2cFAf9ldXvyh",
			Server = "https://parseapi.back4app.com"
		});
	}

	/*
	* Registiert ein User an der Datenbank.
	*/
	public 	Task signUp(string userName, string password, string email){
			var user = new ParseUser()
			{
				Username = userName,
				Password = password,
				Email = email
			};
			Username1 = userName;
			Password1 = password;
			Task signUpTask = null;

			try{
			signUpTask = user.SignUpAsync().ContinueWith(t => {
				if (t.IsFaulted || t.IsCanceled) {
					UnityMainThreadDispatcher.Instance().Enqueue(() => OnRegisterFail(userName,t.Exception));
				} else {
					UnityMainThreadDispatcher.Instance().Enqueue(() => OnRegisterSuccess(userName));
				}
			});
			}
			catch(System.Exception e) {
			}
			return signUpTask;
	}

	/*
	 * Logt einen User ein in der Datenbank und meldet zurück,
	 * ob der Login erfolgreich war oder nicht.
	*/
	public Task signIn(string userName, string password)
	{
		Task task = ParseUser.LogInAsync (userName, password).ContinueWith (t => {
			if (t.IsFaulted || t.IsCanceled) {
				Debug.Log ("Fehler bei der Anmeldung");
				// Sorgt dafür, dass der Aufruf auf dem MainThread gemacht wird. (Christin)
				// OnLoginFail ist der Delegate der ausgeführt wird, um alle die dort angemeldet sind zu benachrichtigen (Christin)
				UnityMainThreadDispatcher.Instance().Enqueue(() => OnLoginFail(userName,t.Exception));
			} else {
				// Sorgt dafür, dass der Aufruf auf dem MainThread gemacht wird. (Christin)
				// OnLoginFail ist der Delegate der ausgeführt wird, um alle die dort angemeldet sind zu benachrichtigen (Christin)
				UnityMainThreadDispatcher.Instance().Enqueue(() => OnLoginSuccess(userName));
			}
		});
		return task;
	}

	/*
	 * Schreibt den Score und den Usernamen in die Datenbank.
	*/
	public Task writeScore(int points) {
		if (ParseUser.CurrentUser != null) {
			ParseObject gameScore = new ParseObject ("GameScore");
			gameScore ["score"] = points;
			gameScore ["playerName"] = ParseUser.CurrentUser.Username;
			Task saveTask = gameScore.SaveAsync ();
			return saveTask;
		}
		return null;
	}

	/*
	 * Gibt die gefüllte Liste mit dem Score und dem Usernamen zurück.
	*/
	public List<ParseObject> getCurrentResultList() {
		return anzeige;
	}

	/*
	 * Fragt an der Datenbank an und zwar die fünf besten Scores des Users und füllt die
	 * Ergebnisse in eine Liste.
	*/
	public void getFiveBestScoreForUser() {
		if (ParseUser.CurrentUser == null)
			return;
		//Anfrage formulieren
		var query = ParseObject.GetQuery("GameScore").WhereEqualTo("playerName", ParseUser.CurrentUser.Username).OrderByDescending("score").Limit(5);

		//Alle Elemente auslesen
		query.FindAsync().ContinueWith(t =>
			{
				if(t.IsCanceled || t.IsFaulted) {
					UnityMainThreadDispatcher.Instance().Enqueue(() => OnUserTop5RequestFailed(t.Exception));
					return;
				}
				anzeige.Clear();
				IEnumerable<ParseObject> results = t.Result;
				int i = 0;
				foreach (ParseObject result in t.Result)
				{
					i+=1;
					anzeige.Add(result);
				}
				UnityMainThreadDispatcher.Instance().Enqueue(() => OnUserTop5Received(anzeige));
			});
	}

	/*
	 * Fragt an der Datenbank an und zwar die fünf besten Scores, die in der Datenbank sind und füllt die
	 * Ergebnisse in eine Liste.
	*/
	public void getFiveBestScore() {

		//Anfrage formulieren
		var query = ParseObject.GetQuery("GameScore").OrderByDescending("score").Limit(5);

		//Alle Elemente auslesen
		query.FindAsync().ContinueWith(t =>
			{
				if(t.IsCanceled || t.IsFaulted) {
					UnityMainThreadDispatcher.Instance().Enqueue(() => OnTop5RequestFailed(t.Exception));
					return;
				}
				anzeige.Clear();
				IEnumerable<ParseObject> results = t.Result;
				int i = 0;
				foreach (ParseObject result in t.Result)
				{
					i+=1;
					anzeige.Add(result);
				}
				UnityMainThreadDispatcher.Instance().Enqueue(() => OnTop5Received(anzeige));
			});
	}

	/*
	 * Logt einen User aus in der Datenbank.
	*/
	public void signOut() {
		if (ParseUser.CurrentUser != null) {
			ParseUser.LogOutAsync ();
		} else 
		{
			Debug.Log ("Ausloggen nicht erfolgreich.");
		
		}


	}

	/*
	 * Gibt den derzeit eingeloggten User zurück.
	*/
	public string getCurrentUser()
	{	
		if (ParseUser.CurrentUser != null) {
			return ParseUser.CurrentUser.Username;
		} else {
			return "null";
		}
	}
}