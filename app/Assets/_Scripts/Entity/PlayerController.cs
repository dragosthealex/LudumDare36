using UnityEngine;
using System.Collections;
//using UnityEngine.Networking;

public class PlayerController : MonoBehaviour {

	public float launchForce; // Launch force. 10 should be kay
	public float walkSpeed; // Walk speed
	public float fireRate; // Fire rate per second
	public GameObject gunPrefab; // Prefab of the gun (the hand)

	private Rigidbody rigBody; // this rigidbody
	private Player player; // The player obj
	private NoGravityPhysicsStuff noGravityScript; // Script that deals with 0 g movemnet
	private LaserGun gun; // The hand
	private AnimController animController; // The animation controller
	public bool movementEnabled;
	public bool canGrab; // True if in a trigger that allows grabbing

	// Use this for initialization
	void Awake () {
		player = GetComponent<Player> ();
		noGravityScript = GetComponent<NoGravityPhysicsStuff> ();
		gun = gunPrefab.GetComponent<LaserGun> ();
		animController = GetComponent<AnimController> ();
		rigBody = GetComponent<Rigidbody> ();
		canGrab = false;	
	}

	void Update () {
		if (GameManager.instance.isPaused) {
			player.GetComponent<MouseLook> ().enabled = false;
			return;
		} else {
			player.GetComponent<MouseLook> ().enabled = true;
		}

		// Move
		if (movementEnabled) {
			if (rigBody.useGravity) {
				MoveWithGravity ();
			} else if (player.isGrabbed && Input.GetKeyDown (KeyCode.W)) {
				grabbyStuffUngrab ();

			}
		}

		// Start shooting
		if (Input.GetKeyDown (KeyCode.Mouse0) && Input.GetKey(KeyCode.Mouse1)) {
			StopCoroutine ("Fire");
			StartCoroutine ("Fire");
		}
		// Aim
		if (Input.GetKeyDown (KeyCode.Mouse1)) {
			animController.SetShooting (true);
		} else if (Input.GetKeyUp (KeyCode.Mouse1)) {
			animController.SetShooting (false);
		}

		// Grab to stuff
		if (Input.GetKeyDown (KeyCode.Space)) {
			if (canGrab) {
				// Grab to the object
				grabbyStuff();
			}
		}

		// Keep grabbed
		// TODO 
		/*
		if (player.isGrabbed && player.grabbedTo) {
			keepGrabbed ();
		}*/
	}

	private void keepGrabbed() {
		// TODO
		//transform.position = (player.grabbedTo.transform.position - player.grabbedOffset) + player.grabbedOffset;
		//Transform grTo = player.grabbedTo.transform;
		//transform.position = new Vector3 (grTo.position.x, grTo.position.y, grTo.position.z - player.grabbedDistance);
		//transform.position = (transform.position - player.grabbedTo.transform.position).normalized 
			// * player.grabbedDistance + player.grabbedTo.transform.position;
		//transform.rotation = Quaternion.Euler(player.grabbedTo.transform.rotation.eulerAngles - player.grabbedOffsetRot.eulerAngles);
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
			canGrab = true;
			player.canGrabTo = col.gameObject;
		} else if (objTag == "gravity_area") {
			TheUI.instance.showTextScript.DisplayText ("After falling through a pothole, you find yourself in the main" +
				" room of some kind of an ancient temple. You notice a weak pulsating light coming from a crystal in the" +
				" middle of the room. You can move with W and S, using the mouse for orientation.", 15f, col.gameObject);
		} else if (objTag == "tunnel_exit") {
			TheUI.instance.showTextScript.DisplayText ("You feel a strange attraction towards the crystal. " +
				"You also notice you are feeling lighter and lighter as you get closer...", 10f, col.gameObject);
		} else if (objTag == "near_crystal") {
			TheUI.instance.showTextScript.DisplayText ("The crystal seems to give you power while it activates." +
			" You feel the the gravity loses its force, and you want to try your new powers aiming with Left Mouse Button" +
			"and shooting with the right. If you get your focus hard, you can aim to different surfaces and push yourself using " +
			"W. When you are close to a grabbable surface, press space to grab.", 15f, col.gameObject);
			activateCrystal ();
		}
	}

	void OnTriggerExit (Collider col) {
		string objTag = col.gameObject.tag;

		if (objTag == "launchygrabby") {
			canGrab = false;
			player.canGrabTo = null;
			player.grabbedTo = null;
		} else if (objTag == "gravity_area") {
			deActivateGravity ();
		}
	}

	// Grab to object
	private void grabbyStuff() {
		noGravityScript.Stop ();
		animController.SetGrab ();

		player.isGrabbed = true;
		player.grabbedTo = player.canGrabTo;
		player.canGrabTo = null;
		canGrab = false;

		// Disable collider and make kinematic
		//animController.animatedModel.GetComponent<CapsuleCollider>().enabled = false;
		//rigBody.isKinematic = true;
		if (player.grabbedTo.GetComponentInParent<Rigidbody> ()) {
			player.grabbedTo.GetComponentInParent<Rigidbody> ().isKinematic = true;
		}
		// Save offset between this and the grabbedTo
		// player.grabbedOffset = player.grabbedTo.transform.position - transform.position;
		// player.grabbedDistance = Vector3.Distance (player.grabbedTo.transform.position, transform.position);
		// player.grabbedOffsetRot = Quaternion.Euler(player.grabbedTo.transform.rotation.eulerAngles - transform.rotation.eulerAngles);
	}

	// Ungrab from object
	private void grabbyStuffUngrab() {
		animController.SetLaunch ();

		player.isGrabbed = false;
		player.canGrabTo = null; // Should be already null
		player.grabbedTo = null;

		// Enable collider and remove kinematic
		animController.animatedModel.GetComponent<CapsuleCollider>().enabled = true;
		rigBody.isKinematic = false;

		noGravityScript.Launch (Camera.main.transform.forward * launchForce);
	}

	public void activateGravity() {
		rigBody.useGravity = true;
		Camera.main.GetComponent<LayersHelper> ().Toggle ("invisible_gravity");
		gameObject.GetComponentInChildren<CapsuleCollider> ().radius = 0.64f;
	}

	public void deActivateGravity() {
		rigBody.useGravity = false;
		Camera.main.GetComponent<LayersHelper> ().Toggle ("invisible_gravity");
		gameObject.GetComponentInChildren<CapsuleCollider> ().radius = 0.24f;
	}

	private void activateCrystal() {
		deActivateGravity ();
		movementEnabled = false;
		StartCoroutine ("activateCrystalCoroutine");
	}

	IEnumerator activateCrystalCoroutine() {
		float time = 0f;
		GameObject crystal = GameObject.FindGameObjectWithTag ("theCrystal");
		crystal.GetComponent<Crystal> ().activate();
		// Put the player up
		// Also put the crystal up
		while (time < 5f) {
			time += Time.deltaTime;
			transform.Translate (Vector3.up * Time.deltaTime * 2f, Space.World);
			crystal.transform.Translate (Vector3.up * Time.deltaTime * 1f, Space.World);
			yield return null;
		}
		// Enable movement
		movementEnabled = true;
		// Activate crystal
	}
}
