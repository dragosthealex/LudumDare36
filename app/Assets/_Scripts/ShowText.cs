using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShowText : MonoBehaviour {

	public ShowPanels panelScript;
	public Text theText;
	public float textTime;

	void Awake () {
		panelScript = GetComponent<ShowPanels> ();
		theText = panelScript.textPanel.GetComponentInChildren<Text> ();
	}

	public void DisplayText(string text) {
		StopCoroutine ("ShowTheText");
		StartCoroutine ("ShowTheText", text);
	}

	IEnumerator ShowTheText(string text) {
		panelScript.ShowTextPanel ();
		theText.text = text;

		yield return new WaitForSeconds(textTime);

		theText.text = "";
		panelScript.HideTextPanel ();
	}
}
