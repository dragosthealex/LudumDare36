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
			// Maria
			// Spawn
			GameObject maria = Instantiate (GameManager.instance.mariaPF, 
				                   GameManager.instance.mariaSpawn.transform.position, 
				                   new Quaternion (0, 0, 0, 0)) as GameObject;
			GameManager.instance.maria = maria;
			print(maria.activeSelf);
			break;
		case 2:
			// Dragos
			break;
		}

	}
}
