using UnityEngine;
using System.Collections;

public class LaserGun : MonoBehaviour {

	public GameObject bulletPrefab;
	public Transform tip;

	public void  Fire() {
		StopCoroutine ("FireLaser");
		StartCoroutine ("FireLaser");

		RaycastHit hit;
		Debug.DrawRay (tip.position, tip.forward * 50);
		if (Physics.Raycast (tip.position, tip.forward, out hit, 50)) {
			print (hit.collider.gameObject.name);
		}
	}

	IEnumerator FireLaser() {
		while (Input.GetKey (KeyCode.Mouse0)) {
			GameObject bullet = Instantiate (bulletPrefab, tip.position, tip.rotation) as GameObject;
			yield return new WaitForSeconds(1f);
		}
	}
}
