﻿using UnityEngine;
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
			// Set start scene in ui and show menu panel
			TheUI.instance.startScript.inMainMenu = true;
			TheUI.instance.panelsScript.ShowMenu ();
			break;
		case 1:
			// Training room
			// Setup player
			gameManager.hideMouse ();
			gameManager.SP_spawn = GameObject.FindGameObjectWithTag ("Respawn").transform;
			gameManager.player = Instantiate (gameManager.playerPF, gameManager.SP_spawn.position, gameManager.SP_spawn.rotation) as GameObject;
			gameManager.player.GetComponent<PlayerController> ().activateGravity ();
			gameManager.player.GetComponent<PlayerController> ().movementEnabled = true;
			// Set start scene false in ui
			TheUI.instance.startScript.inMainMenu = false;
			TheUI.instance.panelsScript.HideMenu ();
			break;	
		case 2:
			// In game screen
			GameManager.instance.hideMouse ();
			break;
		}
	}
}
