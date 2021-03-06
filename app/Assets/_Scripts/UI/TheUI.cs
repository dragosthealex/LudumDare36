﻿using UnityEngine;
using System.Collections;

public class TheUI : MonoBehaviour {

	public static TheUI instance;

	public PanelsManager panelsScript;
	public StartOptions startScript;
	public ShowText showTextScript;

	void Awake () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (this.gameObject);
		}

		DontDestroyOnLoad (gameObject);

		panelsScript = GetComponent<PanelsManager> ();
		startScript = GetComponent<StartOptions> ();
		showTextScript = GetComponent<ShowText> ();
	}
}
