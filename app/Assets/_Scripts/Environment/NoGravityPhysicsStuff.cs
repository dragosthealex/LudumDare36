using UnityEngine;
using System.Collections;

public class NoGravityPhysicsStuff : MonoBehaviour {

	public Vector3 velocity;

	private Rigidbody rigBody;

	void Awake() {
		rigBody = GetComponent<Rigidbody> ();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (velocity.magnitude < 1) {
			velocity = Vector3.zero;
		}
	}

	public void Stop() {
		rigBody.angularVelocity = Vector3.zero;
		rigBody.velocity = Vector3.zero;
		velocity = Vector3.zero;
	}

	public void Launch(Vector3 velocity) {
		this.velocity = velocity;
		rigBody.AddForce (velocity, ForceMode.VelocityChange);
	}

}
