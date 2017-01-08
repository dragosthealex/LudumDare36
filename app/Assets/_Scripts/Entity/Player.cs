using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

	public float launchForce; // Launch force. 10 should be kay
	public float walkSpeed; // Walk speed

	public bool movementEnabled; // I think this should be false :/
	public bool canGrab; // True if in a trigger that allows grabbing
	public bool isGrabbed; // True when grabbed to something
	public GameObject canGrabTo; // What can I grab to?
	public GameObject grabbedTo; // What am I grabbed to right now?
	public Vector3 grabbedOffset; // ????
	public Quaternion grabbedOffsetRot; // ???????????
	public float grabbedDistance; // Wtf was I thinking when coded these???

	private Rigidbody rigBody; // this rigidbody
	private NoGravityPhysicsStuff noGravityScript; // Script that deals with 0 g movemnet
	private AnimController animController; // The animation controller

	// Use this for initialization
	void Awake () {
		// Assign some vars
		noGravityScript = GetComponent<NoGravityPhysicsStuff> ();
		animController = GetComponent<AnimController> ();
		rigBody = GetComponent<Rigidbody> ();
		canGrab = false;
	}

	public override void OnStartLocalPlayer() {
		GameManager.instance.isMultiplayer = true;
	}

	void Update () {
		
		// Move
		if (movementEnabled && !(GameManager.instance.isPaused && GameManager.instance.isMultiplayer)) {
			if (rigBody.useGravity) {
				// When there is gravity, move normally (on floor)
				MoveWithKeys ();
			} else {
				if (GameManager.instance.dev) {
					// In dev mode we can move however we want (in air)
					MoveWithKeys ();
					if (Input.GetKey (KeyCode.LeftAlt)) {
						noGravityScript.Stop ();
					}
				}
				// Ungrab from stuff
				if (isGrabbed && Input.GetKeyDown (KeyCode.W)) {
					grabbyStuffUngrab ();
				}
				// Grab to stuff
				if (Input.GetKeyDown (KeyCode.Space)) {
					if (canGrab) {
						// Grab to the object
						grabbyStuff();
					}
				}


			}
		}


		// Keep grabbed
		// TODO 
		/*
		if (isGrabbed && grabbedTo) {
			keepGrabbed ();
		}*/
	}



	private void keepGrabbed() {
		// TODO
		//transform.position = (grabbedTo.transform.position - grabbedOffset) + grabbedOffset;
		//Transform grTo = grabbedTo.transform;
		//transform.position = new Vector3 (grTo.position.x, grTo.position.y, grTo.position.z - grabbedDistance);
		//transform.position = (transform.position - grabbedTo.transform.position).normalized 
			// * grabbedDistance + grabbedTo.transform.position;
		//transform.rotation = Quaternion.Euler(grabbedTo.transform.rotation.eulerAngles - grabbedOffsetRot.eulerAngles);
	}

	private void MoveWithKeys() {
		transform.Translate (0, 0, Input.GetAxis ("Vertical")*Time.deltaTime * walkSpeed);
	}


	void OnTriggerEnter (Collider col) {
		string objTag = col.gameObject.tag;

		// TODO: replace collider grabby with raycast grabby
		if (objTag == "launchygrabby") {
			canGrab = true;
			canGrabTo = col.gameObject;
			// Show the "Can grab" to the player
			TheUI.instance.panelsScript.ShowGrabInfo (true, "Can grab");
		} else if (objTag == "tunnel_enter") {
			TheUI.instance.showTextScript.DisplayText ("After falling through a pothole, you find yourself in the main" +
				" room of some kind of an ancient temple. You notice a weak pulsating light coming from a crystal in the" +
				" middle of the room. You can move with W and S, using the mouse for orientation.", 10, col.gameObject);
		} else if (objTag == "tunnel_exit") {
			TheUI.instance.showTextScript.DisplayText ("You feel a strange attraction towards the crystal. " +
				"You also notice you are feeling lighter and lighter as you get closer...", 5, col.gameObject);
		} else if (objTag == "near_crystal") {
			TheUI.instance.showTextScript.DisplayText ("The crystal seems to give you power while it activates." +
			" You feel the the gravity loses its force.Try your new powers aiming with Left Mouse Button" +
			" and shooting with the right. You can aim to different surfaces and push yourself using " +
			"W. When you are close to a grabbable surface, press space to grab.", 10, col.gameObject);
			activateCrystal ();
		}
	}

	void OnTriggerExit (Collider col) {
		string objTag = col.gameObject.tag;

		if (objTag == "launchygrabby") {
			// Hide the "Can grab" indication
			TheUI.instance.panelsScript.ShowGrabInfo (false, "");
			canGrab = false;
			canGrabTo = null;
			grabbedTo = null;
		} else if (objTag == "spawnExit") {
			deActivateGravity ();
		}
	}

	// Grab to object
	private void grabbyStuff() {
		noGravityScript.Stop ();
		animController.SetGrab ();

		isGrabbed = true;
		grabbedTo = canGrabTo;
		canGrabTo = null;
		canGrab = false;

		// Show that we grabbed
		TheUI.instance.panelsScript.ShowGrabInfo (true, "Grabbed");

		// Disable collider and make kinematic
		//animController.animatedModel.GetComponent<CapsuleCollider>().enabled = false;
		//rigBody.isKinematic = true;
		if (grabbedTo.GetComponentInParent<Rigidbody> ()) {
			grabbedTo.GetComponentInParent<Rigidbody> ().isKinematic = true;
		}
		// Save offset between this and the grabbedTo
		// grabbedOffset = grabbedTo.transform.position - transform.position;
		// grabbedDistance = Vector3.Distance (grabbedTo.transform.position, transform.position);
		// grabbedOffsetRot = Quaternion.Euler(grabbedTo.transform.rotation.eulerAngles - transform.rotation.eulerAngles);
	}

	// Ungrab from object
	private void grabbyStuffUngrab() {
		animController.SetLaunch ();

		isGrabbed = false;
		canGrabTo = null; // Should be already null
		grabbedTo = null;

		// We ungrabbed
		// Show that we grabbed
		TheUI.instance.panelsScript.ShowGrabInfo (false, "");

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

	public void ToggleMovement(bool toggle) {
		movementEnabled = toggle;
	}

	public void ToggleMouseLook(bool toggle) {
		GetComponent<MouseLook> ().enabled = toggle;
	}

	// Destroys this script for remote stuff
	public void DestroyScript () {
		Destroy (this);
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
