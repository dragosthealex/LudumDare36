using UnityEngine;
using System.Collections;
//using UnityEngine.Networking;

public class PlayerController : MonoBehaviour {

	public float launchForce;
	public float walkSpeed;
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
		rigBody = GetComponent<Rigidbody> ();
	}

	void Update () {
		if (GameManager.instance.isPaused) {
			return;
		}

		// Move
		if (rigBody.useGravity) {
			MoveWithGravity ();
		} else if (player.isGrabbed && Input.GetKeyDown (KeyCode.W)) {
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

	private void MoveWithGravity() {
		transform.Translate (0, 0, Input.GetAxis ("Vertical") * walkSpeed);
	}

	IEnumerator Fire() {
		while (Input.GetKey (KeyCode.Mouse0) && Input.GetKey(KeyCode.Mouse1)) {
			animController.SetShoot ();
			gun.Fire ();
			yield return new WaitForSeconds(fireRate);
		}
	}

	void OnTriggerEnter (Collider col) {
		string objTag = col.gameObject.tag;

		if (objTag == "launchygrabby") {
			player.isGrabbed = true;
			noGravityScript.Stop ();
		} else if (objTag == "gravity_area") {
			activateGravity ();
		}
	}

	void OnTriggerExit (Collider col) {
		string objTag = col.gameObject.tag;

		if (objTag == "launchygrabby") {
			player.isGrabbed = false;
		} else if (objTag == "gravity_area") {
			deActivateGravity ();
		}
	}

	private void activateGravity() {
		rigBody.useGravity = true;
		Camera.main.GetComponent<LayersHelper> ().Toggle ("invisible_gravity");
		gameObject.GetComponentInChildren<CapsuleCollider> ().radius = 0.64f;
	}

	private void deActivateGravity() {
		rigBody.useGravity = false;
		Camera.main.GetComponent<LayersHelper> ().Toggle ("invisible_gravity");
		gameObject.GetComponentInChildren<CapsuleCollider> ().radius = 0.24f;
	}
}
