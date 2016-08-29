using UnityEngine;
using System.Collections;
//using UnityEngine.Networking;

public class PlayerController : MonoBehaviour {

	public float launchForce;
	public float fireRate;
	public GameObject gunPrefab;

	private Rigidbody rigBody;
	private Player player;
	private NoGravityPhysicsStuff noGravityScript;
	private LaserGun gun;
	private AnimController animController;

	// Use this for initialization
	void Awake () {
		player = GetComponent<Player> ();
		noGravityScript = GetComponent<NoGravityPhysicsStuff> ();
		gun = gunPrefab.GetComponent<LaserGun> ();
		animController = GetComponent<AnimController> ();
	}

	void Update () {
		if (GameManager.instance.isPaused) {
			return;
		}

		// Move
		if (player.isGrabbed && Input.GetKeyDown (KeyCode.W)) {
			noGravityScript.Launch (Camera.main.transform.forward * launchForce);
			player.isGrabbed = false;
		}

		// Start shooting
		if (Input.GetKeyDown (KeyCode.Mouse0) && Input.GetKey(KeyCode.Mouse1)) {
			StopCoroutine ("Fire");
			StartCoroutine ("Fire");
		}

		if (Input.GetKeyDown (KeyCode.Mouse1)) {
			animController.SetShooting (true);
		} else if (Input.GetKeyUp (KeyCode.Mouse1)) {
			animController.SetShooting (false);
		}
	}

	IEnumerator Fire() {
		while (Input.GetKey (KeyCode.Mouse0) && Input.GetKey(KeyCode.Mouse1)) {
			animController.SetShoot ();
			gun.Fire ();
			yield return new WaitForSeconds(fireRate);
		}
	}

	void OnTriggerEnter (Collider col) {
		if (col.gameObject.tag == "launchygrabby") {
			player.isGrabbed = true;
			noGravityScript.Stop ();
		}
	}

	void OnTriggerExit (Collider col) {
		if (col.gameObject.tag == "launchygrabby") {
			player.isGrabbed = false;
		}
	}

}
