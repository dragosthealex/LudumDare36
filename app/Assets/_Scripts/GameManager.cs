using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null; // The singleton instance
	public static SceneLevelManager sceneScript = null;

	public bool isPaused = false; // Whether game is paused
	public bool dev = false; // Whether we are in developing mode or not

	public GameObject playerPF;
	public GameObject player;
	public Transform SP_spawn;

	private bool menuActive;

	public int level;

	void Awake () {
		// Ensure Singleton pattern, GameManager should exist only once
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (this.gameObject);
		}
		// Assign some vars
		sceneScript = GetComponent<SceneLevelManager> ();
		level = 0;
		// Prevent the GameManager from being destroyed when changing scenes
		DontDestroyOnLoad (gameObject);
		// Set the scene loading function
		// When changing scenes, we assign the level, unpause and initialise the game again
		SceneManager.sceneLoaded += (FindSceneObjectsOfType, loadingMode) => {
			level = SceneManager.GetActiveScene ().buildIndex;
			isPaused = false;
			InitGame ();
		};
	}

	// Initialise the game in a new scene
	void InitGame() {
		// Setup the current scene
		sceneScript.setupScene (level);
		// Hide the editor only objects
		foreach(GameObject obj in GameObject.FindGameObjectsWithTag("editor_only")) {
			obj.SetActive(false);
		}
	}

	// Restart the game to original scene
	public void RestartGame() {
		SceneManager.LoadScene (0);
	}

	// Show mouse cursor
	public void showMouse() {
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	// Hide mouse cursro
	public void hideMouse() {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	void Update() {

	}
}
