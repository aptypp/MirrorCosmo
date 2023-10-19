using Mirror;
using UnityEngine;

namespace MomoCoop
{
    public sealed class GameClient : MonoBehaviour
    {
        [SerializeField]
        private NetworkRoomManager _networkRoomManager;

        private GameNetworkRoomPlayer _networkRoomPlayer;

        public void ConnectToServer(string ip)
        {
            _networkRoomManager.networkAddress = ip;

            _networkRoomManager.StartClient();
        }

        public void SetNetworkRoomPlayer(GameNetworkRoomPlayer networkRoomPlayer) => _networkRoomPlayer = networkRoomPlayer;

        public void SetIsReady(bool isReady)
        {
            _networkRoomPlayer.CmdChangeReadyState(isReady);
        }

        public void Disconnect()
        {
            _networkRoomManager.StopClient();
        }
    }
}