using UnityEngine;
using System.Collections;

/// MouseLook rotates the transform based on the mouse delta.
/// Minimum and Maximum values can be used to constrain the possible rotation

/// To make an FPS style character:
/// - Create a capsule.
/// - Add the MouseLook script to the capsule.
///   -> Set the mouse look to use LookX. (You want to only turn character but not tilt it)
/// - Add FPSInputController script to the capsule
///   -> A CharacterMotor and a CharacterController component will be automatically added.

/// - Create a camera. Make the camera a child of the capsule. Reset it's transform.
/// - Add a MouseLook script to the camera.
///   -> Set the mouse look to use LookY. (You want the camera to tilt up and down like a head. The character already turns.)
[AddComponentMenu("Camera-Control/Mouse Look")]
public class MouseLook : MonoBehaviour {
	public float mouseSensitivity = 150.0f;

	private float rotY = 0.0f; // rotation around the up/y axis
	private float rotX = 0.0f; // rotation around the right/x axis
	private float rotZ = 0.0f;

	private float oldrotX = 0.0f;
	private float mouseX;
	private float mouseY;
	// While is 1, it means we are in normal position. When we get upside down, it 
	// changes to -1, to keep left and right the same.
	private int upsideDownFlag = 1;

	void Start () {
		Vector3 rot = transform.localRotation.eulerAngles;
		rotY = rot.y;
		rotX = rot.x;

		if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;
	}

	void LateUpdate () {

		if (GameManager.instance.isPaused && GameManager.instance.isMultiplayer) {
			return;
		}

		mouseX = Input.GetAxis("Mouse X");
		mouseY = -Input.GetAxis("Mouse Y");

		rotY = upsideDownFlag * mouseX * mouseSensitivity * Time.deltaTime;
		rotX = mouseY * mouseSensitivity * Time.deltaTime;

		if (rotY > 180) {
			rotY = - 180 + rotY % 180;
		} else if (rotY < -180) {
			rotY = 360 + rotY;
		}
		if (rotX > 180) {
			rotX = - 180 + rotX % 180;
		} else if (rotX < -180) {
			rotX = 360 + rotX;

		}
	
		if (rotX >= -90 && rotX <= 90) {
			upsideDownFlag = 1;
		} else {
			upsideDownFlag = -1;
		}

		oldrotX = rotX;


		if (Input.GetKey (KeyCode.Q)) {
			rotZ = GameManager.instance.walkSpeed/2 * Time.deltaTime * 100;			
		} else if (Input.GetKey (KeyCode.E)) {
			rotZ = -GameManager.instance.walkSpeed/2 * Time.deltaTime * 100;
		} else {
			rotZ = 0;
		}

		transform.Rotate(new Vector3(rotX, rotY, rotZ), Space.Self);
	}
}