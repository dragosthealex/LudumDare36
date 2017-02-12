using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PanelsManager : MonoBehaviour {

	/* Panels in following order:
	1. menu
	2. in game
	3. pause
	4. text
	5. options
	6 ...
	*/
	public GameObject[] panels;

	public enum PanelNames {
		MENU,
		INGAME,
		PAUSE,
		TEXT,
		LOBBY,
		LOBBY_TOP,
	}

	private Text grabInfo;

	public void Start() {
		grabInfo = panels[(int) PanelNames.INGAME].GetComponentInChildren<Text> ();
		grabInfo.gameObject.SetActive(false);
	}

	// Show info about "grabbing status"
	// TODO: Make this cooler.
	public void ShowGrabInfo(bool show, string text) {
		grabInfo.gameObject.SetActive(show);
		grabInfo.text = text;
	}

	// Toggle a panel to be active/inactive
	public void TogglePanel(string panelName, bool active) {
		panels [(int) System.Enum.Parse (typeof(PanelNames), panelName)].SetActive (active);
	}
	public void TogglePanel(PanelNames panelNo, bool active) {
		panels [(int) panelNo].SetActive (active);
	}

	// Hides all panels
	public void HideAll() {
		foreach (PanelNames panelNo in System.Enum.GetValues(typeof(PanelNames))) {
			TogglePanel (panelNo, false);
		}
	}
}