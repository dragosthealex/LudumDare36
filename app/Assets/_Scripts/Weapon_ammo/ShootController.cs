﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ShootController : NetworkBehaviour {

	public Transform gunPosition; // The fire position
	public float fireRate; // Fire rate per second
	public float range;
	public bool canShoot;
	float elapsedTime;
	public LaserWeapon weaponEffects;

	private AnimController animController;

	void Awake () {
		animController = GetComponent<AnimController> ();
	}

	// Use this for initialization
	void Start () {
		if (isLocalPlayer) {
			canShoot = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!canShoot) {
			return;
		}
		elapsedTime += Time.deltaTime;

		// Start shooting
		if (Input.GetKeyDown (KeyCode.Mouse0) && Input.GetKey(KeyCode.Mouse1)) {
			if (elapsedTime >= fireRate) {
				animController.SetShoot ();
				elapsedTime = 0;
				CmdFireShot (gunPosition.position, gunPosition.forward, gunPosition.rotation);
				weaponEffects.Fire (gunPosition);
			}
		}
		// Aim
		if (Input.GetKeyDown (KeyCode.Mouse1)) {
			animController.SetShooting (true);
		} else if (Input.GetKeyUp (KeyCode.Mouse1)) {
			animController.SetShooting (false);
		}
	}

	[Command]
	public void CmdFireShot(Vector3 origin, Vector3 direction, Quaternion rotation) {

		RaycastHit hit;

		Ray ray = new Ray (origin, direction);
		Debug.DrawRay (ray.origin, ray.direction * 3f, Color.red, 1f);

		// If we hit something, add force to it (and maybe deal damage)
		bool result = Physics.Raycast (ray, out hit, range);
		if (result && hit.collider.GetComponentInParent<NetworkIdentity>() != null) {
			RpcAddForce (hit.point, hit.collider.GetComponentInParent<NetworkIdentity>().netId);
		}
		RpcProcessShot (result, Vector3.zero, origin, rotation);
	}

	[ClientRpc]
	public void RpcAddForce(Vector3 point, NetworkInstanceId id) {
		ClientScene.FindLocalObject (id).gameObject.GetComponent<Rigidbody>()
									.AddExplosionForce (40f, point, 10f, 0f, ForceMode.Impulse);
	}

	[ClientRpc]
	public void RpcProcessShot(bool playImpact, Vector3 point, Vector3 origin, Quaternion rotation) {
		weaponEffects.Fire (origin, rotation);
	}
}
