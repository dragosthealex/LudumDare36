using UnityEngine;
using System.Collections;
//using UnityEngine.Networking;

public class PlayerController : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (GameManager.instance.isPaused)
		{
			return;
		}
		// Move
		var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
		var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;
		transform.Rotate(0, x, 0);
		transform.Translate(0, 0, z);

		// Jump

		// Say name
		if (Input.GetKey (KeyCode.N)) {
			print (name);
		}

	}
}
