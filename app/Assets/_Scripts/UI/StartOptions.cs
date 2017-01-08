using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;


public class StartOptions : MonoBehaviour {

	public bool inMainMenu = true;
	private PanelsManager showPanels;

	
	void Awake() {
		//Get a reference to ShowPanels attached to UI object
		showPanels = GetComponent<PanelsManager> ();
	}


	// When a button that changes a scene is pressed (e.g. training room, or start)
	public void ChangeAScene(int sceneNo) {
		inMainMenu = false;
		SceneManager.LoadSceneAsync (sceneNo);
	}

	void OnLevelWasLoaded() {
		//
	}
}
