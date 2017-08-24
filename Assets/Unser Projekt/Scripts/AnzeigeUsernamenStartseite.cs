using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Parse;
using TMPro;

public class AnzeigeUsernamenStartseite : MonoBehaviour {
	
	TextMeshProUGUI TextfeldName;
	string oldUser = "";
	// Use this for initialization
	void Start () {

		TextfeldName = GetComponent<TextMeshProUGUI> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!Datenbankverwaltung.instance.getCurrentUser ().Equals (oldUser)) {
			TextfeldName.SetText (Datenbankverwaltung.instance.getCurrentUser ());
			oldUser = Datenbankverwaltung.instance.getCurrentUser ();
		}	
	}
}
