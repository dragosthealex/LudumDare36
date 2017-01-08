using UnityEngine;
using System.Collections;

public class LaserWeapon : RangedWeapon {

	public GameObject bulletPrefab;

	public void  Fire(Vector3 origin, Quaternion rotation) {
		GameObject bullet = Instantiate (bulletPrefab, origin, rotation) as GameObject;
	}
}
