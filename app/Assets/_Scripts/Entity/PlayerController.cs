using UnityEngine;
using System.Collections;
//using UnityEngine.Networking;

public class PlayerController : MonoBehaviour {

	public float forwardSpeed;
	public float sidewaysSpeed;
	public float moveForce;

	private Rigidbody rigBody;

	// Use this for initialization
	void Awake () {
		rigBody = GetComponent<Rigidbody> ();
	}

	void Update () {
		if (GameManager.instance.isPaused) {
			return;
		}

		// Move
		var z = Input.GetAxisRaw("Vertical") * Time.deltaTime * forwardSpeed;
		//transform.Translate(0, 0, z);
		rigBody.AddForce (Camera.main.transform.forward * z);
	}
}
