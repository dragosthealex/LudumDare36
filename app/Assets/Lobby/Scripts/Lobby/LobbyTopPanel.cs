using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Prototype.NetworkLobby
{
    public class LobbyTopPanel : MonoBehaviour
    {
        public bool isInGame = false;

        void Update()
        {
            if (!isInGame)
                return;
        }
    }
}