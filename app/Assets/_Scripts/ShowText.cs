using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShowText : MonoBehaviour {

	public ShowPanels panelScript;
	public Text theText;
	public float textTime;

	void Awake () {
		panelScript = TheUI.instance.panelsScript;
		theText = panelScript.textPanel.GetComponentInChildren<Text> ();
	}

	// Display a text with default time
	public void DisplayText(string text) {
		StopCoroutine ("ShowTheText");
		StartCoroutine ("ShowTheText", new object[2]{text, textTime});
	}

	// Display a text with a time
	public void DisplayText(string text, float time) {
		StopCoroutine ("ShowTheText");
		StartCoroutine ("ShowTheText", new object[2]{text, time});
	}

	// Display a text with time and destroy the game object
	public void DisplayText(string text, float time = 5f, GameObject obj = null) {
		StopCoroutine ("ShowTheText");
		StartCoroutine("ShowTheText", new object[2]{text, time});

		Destroy (obj);
	}

	IEnumerator ShowTheText(object[] parameters) {
		panelScript.ShowTextPanel ();
		theText.text = (string)parameters[0];

		yield return new WaitForSeconds((float)parameters[1]);

		theText.text = "";
		panelScript.HideTextPanel ();
	}
}
