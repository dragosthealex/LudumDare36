using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour {


	private PanelsManager showPanels;						//Reference to the ShowPanels script used to hide and show UI panels
	private StartOptions startScript;					//Reference to the StartButton script

	private GameManager manager;
	//Awake is called before Start()
	void Awake()
	{
		//Get a component reference to ShowPanels attached to this object, store in showPanels variable
		showPanels = GetComponent<PanelsManager> ();
		//Get a component reference to StartButton attached to this object, store in startScript variable
		startScript = GetComponent<StartOptions> ();
		manager = GameManager.instance;
	}

	// Update is called once per frame
	void Update () {
		//Check if the Cancel button in Input Manager is down this frame (default is Escape key) and that game is not paused, and that we're not in main menu
		if (Input.GetButtonDown ("Cancel") && !manager.isPaused && !startScript.inMainMenu) 
		{
			//Call the DoPause function to pause the game
			DoPause();
		} 
		//If the button is pressed and the game is paused and not in main menu
		else if (Input.GetButtonDown ("Cancel") && manager.isPaused && !startScript.inMainMenu) 
		{
			//Call the UnPause function to unpause the game
			UnPause ();
		}
	
	}

	// Pause the game (show the 'pause' panel)
	public void DoPause()
	{
		
		//call the ShowPausePanel function of the ShowPanels script
		showPanels.TogglePanel (PanelsManager.PanelNames.PAUSE, true);
		manager.isPaused = true;
		manager.showMouse ();
		if (!manager.isMultiplayer) {
			//Set time.timescale to 0, this will cause animations and physics to stop updating
			Time.timeScale = 0;
		}
	}

	// Unpause the game (hide the 'pause' panel)
	public void UnPause()
	{
		//call the HidePausePanel function of the ShowPanels script
		showPanels.TogglePanel (PanelsManager.PanelNames.PAUSE, false);
		manager.isPaused = false;
		manager.hideMouse ();
		if (!manager.isMultiplayer) {
			//Set time.timescale to 0, this will cause animations and physics to stop updating
			Time.timeScale = 1;
		}
	}

	// Restart the game, going to the first scene
	public void Restart() {
		if (!manager.isMultiplayer) {
			Time.timeScale = 1;
		}
		showPanels.TogglePanel (PanelsManager.PanelNames.PAUSE, false);
		startScript.inMainMenu = true;
		manager.RestartGame ();
	}

}
