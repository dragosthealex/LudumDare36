using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null; // The singleton instance
	public static SceneLevelManager sceneScript = null;

	public bool isPaused = false; // Whether game is pause

	public GameObject playerPF;
	public GameObject player;
	public Transform SP_spawn;

	private bool menuActive;

	public int level;

	void Awake () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (this.gameObject);
		}

		sceneScript = GetComponent<SceneLevelManager> ();
		level = 0;

		DontDestroyOnLoad (gameObject);

		SceneManager.sceneLoaded += (FindSceneObjectsOfType, loadingMode) => {
			level = SceneManager.GetActiveScene ().buildIndex;
			isPaused = false;
			InitGame ();
		};
	}

	void InitGame() {
		sceneScript.setupScene (level);
	}

	public void RestartGame() {
		SceneManager.LoadScene (0);
	}

	public void showMouse() {
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	public void hideMouse() {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	void Update() {

	}
}
