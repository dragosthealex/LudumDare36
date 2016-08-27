using UnityEngine;
using System.Collections;

public class ElasticCollision : MonoBehaviour {

	private NoGravityPhysicsStuff noGravityScript;

	void Awake() {
		noGravityScript = GetComponent<NoGravityPhysicsStuff> ();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnCollisionEnter (Collision col) {
		GameObject other = col.gameObject;
		Rigidbody rigBody1 = GetComponent<Rigidbody> ();
		Rigidbody rigBody2 = other.GetComponent<Rigidbody> ();

		Vector3 v1 = rigBody1.velocity;
		float m1 = rigBody1.mass;

		print (v1.magnitude);
		if (rigBody2 == null) {
			noGravityScript.velocity = -v1 * 0.8f;
			return;
		}

		Vector3 v2 = rigBody2.velocity;
		float m2 = rigBody2.mass;

		noGravityScript.velocity = ((m1 - m2) / (m1 + m2)) * v1 + ((2 * m2) / (m1 + m2)) * v2;

	}

}
