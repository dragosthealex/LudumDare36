using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

// Events for toggling network-related stuff
[System.Serializable]
public class ToggleEvent : UnityEvent<bool>{}

public class NetworkedPlayer : NetworkBehaviour {

	// Network stuff
	[SerializeField] ToggleEvent onToggleShared;
	[SerializeField] ToggleEvent onToggleLocal;
	[SerializeField] ToggleEvent onToggleRemote;

	void Start() {
		EnablePlayer ();
	}

	// If we are a client
	public override void OnStartClient() {
		
	}

	// If we are a server
	public override void OnStartServer() {


	}

	// Disables this player
	public void DisablePlayer() {
		if (isLocalPlayer) {
			onToggleLocal.Invoke (false);
		} else {
			onToggleRemote.Invoke (false);
		}
	}

	// Enables this player
	public void EnablePlayer() {
		onToggleShared.Invoke (true);

		if (isLocalPlayer) {
			onToggleLocal.Invoke (true);
		} else {
			onToggleRemote.Invoke (true);
		}
	}
}
