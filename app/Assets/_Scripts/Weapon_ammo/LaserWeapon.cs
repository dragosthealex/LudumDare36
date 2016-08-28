using UnityEngine;
using System.Collections;

public class LaserWeapon : RangedWeapon {

	private LineRenderer line;

	// Use this for initialization
	void Start () {
		line = GetComponent<LineRenderer> ();
		line.enabled = false;

	}
	
	// Update is called once per frame
	void Update () {
	
	}
		
	public void  Fire() {
		StopCoroutine ("FireLaser");
		StartCoroutine ("FireLaser");
	}

	IEnumerator FireLaser() {
		line.enabled = true;

		while (Input.GetKey (KeyCode.Mouse0)) {
			line.material.mainTextureOffset = new Vector2 (Time.time, Time.time);
			Ray ray = new Ray (transform.position, transform.forward);
			RaycastHit hit;

			line.SetPosition (0, ray.origin);

			if(Physics.Raycast(ray, out hit, 50)) {
				line.SetPosition(1, hit.point);
				// Hit stuff
				if (hit.rigidbody) {
					hit.rigidbody.AddExplosionForce (2000f, hit.point, 15);
				}
			} else {
				line.SetPosition (1, ray.GetPoint (50));
			}
			yield return null;
		}

		line.enabled = false;
	}
}
