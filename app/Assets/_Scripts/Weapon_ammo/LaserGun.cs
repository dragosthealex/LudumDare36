using UnityEngine;
using System.Collections;

public class LaserGun : MonoBehaviour {

	public GameObject bulletPrefab;
	public Transform tip;

	public void  Fire() {
		StopCoroutine ("FireLaser");
		StartCoroutine ("FireLaser");
	}

	IEnumerator FireLaser() {

		while (Input.GetKey (KeyCode.Mouse0)) {
			GameObject bullet = Instantiate (bulletPrefab, tip.position, tip.rotation) as GameObject;
			/*
			Ray ray = new Ray (transform.position, transform.forward);
			RaycastHit hit;

			if(Physics.Raycast(ray, out hit, 50)) {
				// Hit stuff
				if (hit.rigidbody) {
					hit.rigidbody.AddExplosionForce (2000f, hit.point, 15);
				}
			}*/
			yield return new WaitForSeconds(0.25f);
		}
	}
}
