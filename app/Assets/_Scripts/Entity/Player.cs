﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

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

	// Authoritative MP Stuff
	private int sendStep = 0;
	// TODO: make these controlls modifiable
	private KeyCode[] keys;
	/// <summary>
	/// Keyboard input history, lower indexes are newer
	/// </summary>
	public List<HashSet<KeyCode>> history=new List<HashSet<KeyCode>>();
	/// <summary>
	/// How much history to keep?
	/// history.Count will be max. this value
	/// </summary>
	[Range(1,1000)]
	public int historyLengthInSteps=10;

	// Use this for initialization
	void Awake () {
		// Assign some vars
		noGravityScript = GetComponent<NoGravityPhysicsStuff> ();
		animController = GetComponent<AnimController> ();
		rigBody = GetComponent<Rigidbody> ();
		canGrab = false;
		//
		keys = new KeyCode[6]{KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.Q, KeyCode.E}; 
	}

	public override void OnStartLocalPlayer() {
		GameManager.instance.isMultiplayer = true;
		TheUI.instance.panelsScript.TogglePanel (PanelsManager.PanelNames.INGAME, true);
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
			} // if
		}// if
		Raycasting ();
	}

	private void MoveWithKeys() {
		var keysThisFrame=new HashSet<KeyCode>();
		if (Input.anyKeyDown) {
			foreach (KeyCode kc in keys) {
				if (Input.GetKey (kc)) {
					keysThisFrame.Add (kc);
					Debug.Log ("added " + kc.ToString());
				}
			}
		}
		int count = history.Count - historyLengthInSteps;
		if (count > 0) {
			history.RemoveRange (historyLengthInSteps, count);
		}
		transform.Translate (0, 0, Input.GetAxis ("Vertical")*Time.deltaTime * walkSpeed);
	}

	private void Raycasting() {
		Vector3[] directions = new Vector3[6];

		directions[0] = transform.TransformDirection (Vector3.forward);
		directions[1] = transform.TransformDirection (Vector3.back);
		directions[2] = transform.TransformDirection (Vector3.up);
		directions[3] = transform.TransformDirection (Vector3.down);
		directions[4] = transform.TransformDirection (Vector3.left);
		directions[5] = transform.TransformDirection (Vector3.right);

		RaycastHit hit;
		bool ok = false;
		foreach(Vector3 direction in directions) {
			if (Physics.Raycast (transform.position, direction, out hit, 2)) {
				// Something around us.
				GameObject obj = hit.collider.gameObject;
				if (obj.tag == "launchygrabby") {
					canGrab = true;
					canGrabTo = obj;
					// Show the "Can grab" to the player
					TheUI.instance.panelsScript.ShowGrabInfo (true, "Can grab");
					ok = true;
					break;
				}
			}
		}
		if (!ok) {
			TheUI.instance.panelsScript.ShowGrabInfo (false, "");
		}
	}

	public void FixedUpdate() {
		sendStep++;
		if (sendStep == 5) {
			for (int ago = history.Count - 1; ago >= 0; ago--) {
				var s = string.Format ("{0:0000}:", ago);
				foreach (var k in history[ago])
					s += "\t" + k;
				Debug.Log (s);
			}
			sendStep = 0;
		}
	}
		
	//------------Triggers------------

	void OnTriggerEnter (Collider col) {
		string objTag = col.gameObject.tag;
	}

	void OnTriggerExit (Collider col) {
		string objTag = col.gameObject.tag;

		if (objTag == "launchygrabby") {
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
	}

	// Ungrab from object
	private void grabbyStuffUngrab() {
		animController.SetLaunch ();

		// Hide the "Can grab" indication
		TheUI.instance.panelsScript.ShowGrabInfo (false, "");
		canGrab = false;
		isGrabbed = false;
		canGrabTo = null; // Should be already null
		grabbedTo = null;

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
}
