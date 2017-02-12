using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

[ExecuteInEditMode]

public class AuthoritativeNetworkManager : NetworkBehaviour {

	public Transform player;
	string registeredName = "BattleRoom";
	float refreshRequestLength = 3.0f;
	HostData[] hostData;
	public string chosenGameName = "";
	public NetworkPlayer myPlayer;

}
