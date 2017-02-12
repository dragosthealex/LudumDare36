using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ObjectSpawner : NetworkBehaviour {

	public void Awake() {
		// Spawn this object on awake
		NetworkServer.Spawn(this.gameObject);
	}
}
