using UnityEngine;
using System.Collections;

public class LaserGun : MonoBehaviour {

	public GameObject bulletPrefab;
	public Transform tip;

	public void  Fire() {
		GameObject bullet = Instantiate (bulletPrefab, tip.position, tip.rotation) as GameObject;
	}
}
