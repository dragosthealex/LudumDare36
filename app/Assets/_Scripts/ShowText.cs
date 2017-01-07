using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShowText : MonoBehaviour {

	public PanelsManager panelScript;
	public float textTime;

	private Text theText;
	private bool isShowing = false;
	private float counter = 0f;
	private GameObject currentTrigger = null;

	void Awake () {
		panelScript = TheUI.instance.panelsScript;
		// The text gameobject where we write the text
		theText = panelScript.panels [(int)PanelsManager.PanelNames.TEXT]
			.gameObject.GetComponentInChildren<Text> ();
	}

	// Display a text with time
	public void DisplayText(string text, int time = 5, GameObject trigger = null) {
		// Check if we already show something or not
		if (!isShowing) {
			panelScript.TogglePanel (PanelsManager.PanelNames.TEXT, true);
		} else if (currentTrigger) {
			// If we had a trigger, destroy it after the text is changed
			Destroy (currentTrigger);
		}

		// Change the text to new value
		theText.text = text;
		// Set the counter to time remaining
		counter = (float) time;
		// Set the trigger that 
	}

	// Hide the text, whatever it may be
	public void HideText() {
		counter = 0;
	}

	void Update() {
		// If counter reached 0, hide the panel
		if (counter <= 0) {
			theText.text = "";
			panelScript.TogglePanel (PanelsManager.PanelNames.TEXT, false);
			// Destroy the trigger if it is set
			if (currentTrigger) {
				Destroy (currentTrigger);
			}
			return;
		}
		// Else substract every second
		counter -= Time.deltaTime;
	}
}
