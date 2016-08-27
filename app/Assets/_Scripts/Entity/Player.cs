using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public bool isGrabbed;

	private Rigidbody rigBody;

	// Use this for initialization
	void Awake () {
		rigBody = GetComponent<Rigidbody> ();
		isGrabbed = true;
	}
	
	// Update is called once per frame
	void Update () {

		// Check if not paused
		if (!GameManager.instance.isPaused) {
			UpdatePlayer ();
		}
	}

	private void UpdatePlayer() {
		
	}


}
