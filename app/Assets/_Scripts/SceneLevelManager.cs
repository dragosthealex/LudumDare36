using UnityEngine;
using System.Collections;

public class SceneLevelManager : MonoBehaviour {

	private GameManager gameManager;

	void Awake () {
		gameManager = GameManager.instance.GetComponent<GameManager>();	
	}

	public void setupScene(int level) {
		switch (level) {
		case 0:
			// Start game screen
			// Set start scene in ui, hide other panels and show menu panel
			TheUI.instance.startScript.inMainMenu = true;
			TheUI.instance.panelsScript.HideAll ();
			TheUI.instance.panelsScript.TogglePanel (PanelsManager.PanelNames.MENU, true);

			break;
		case 1:
			// Training room
			// Setup player
			gameManager.hideMouse ();
			// Set start scene false in ui
			TheUI.instance.startScript.inMainMenu = false;
			TheUI.instance.panelsScript.TogglePanel (PanelsManager.PanelNames.MENU, false);
			// Setup the single player
			gameManager.SP_spawn = GameObject.FindGameObjectWithTag ("Respawn").transform;
			gameManager.player = Instantiate (gameManager.playerPF, gameManager.SP_spawn.position, gameManager.SP_spawn.rotation) as GameObject;
			gameManager.player.GetComponent<Player> ().activateGravity ();
			gameManager.player.GetComponent<Player> ().movementEnabled = true;
			gameManager.player.GetComponent<Player> ().ToggleMouseLook (true);


			break;	
		case 2:
			// In game screen (multiplayer)
			// Set start scene false in ui
			TheUI.instance.startScript.inMainMenu = false;
			TheUI.instance.panelsScript.TogglePanel (PanelsManager.PanelNames.MENU, false);
			// Network Manager will do the player stuff
			break;
		case 3:
			// In lobby (multiplayer)
			TheUI.instance.startScript.inMainMenu = false;
			TheUI.instance.panelsScript.HideAll ();
			TheUI.instance.panelsScript.TogglePanel (PanelsManager.PanelNames.LOBBY, true);
			break;
		}
	}
}
