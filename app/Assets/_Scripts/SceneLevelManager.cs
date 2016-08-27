using UnityEngine;
using System.Collections;

public class SceneLevelManager : MonoBehaviour {

	public void setupScene(int level) {
		switch (level) {
		case 0:
			// Start game screen
			// Do nothing
			break;
		case 1:
			// In game screen
			break;
		case 2:
			// Training room
			// Setup player
			GameManager.instance.player = Instantiate (GameManager.instance.playerPF);
			break;
		}
	}
}
