using UnityEngine;
using System.Collections;
//using UnityEngine.Networking;

public class PlayerController : MonoBehaviour {

	public float launchForce;

	private Rigidbody rigBody;
	private Player player;
	private NoGravityPhysicsStuff noGravityScript;

	// Use this for initialization
	void Awake () {
		player = GetComponent<Player> ();
		noGravityScript = GetComponent<NoGravityPhysicsStuff> ();
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
