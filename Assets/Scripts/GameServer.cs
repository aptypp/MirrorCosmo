using Mirror;
using MomoCoop.MapGeneration;
using MomoCoop.NPC;
using UnityEngine;

namespace MomoCoop
{
    public sealed class GameServer : MonoBehaviour
    {
        [SerializeField]
        private Enemy _enemyPrefab;

        [SerializeField]
        private CustomNetworkRoomManager _networkRoomManager;

        private Enemy _enemy;
        private MapGenerator _mapGenerator;

        public void StartServer()
        {
            _networkRoomManager.StartServer();
            _networkRoomManager.gameSceneLoaded += StartGame;
            _networkRoomManager.allClientsDisconnected += TryReturnToRoom;
        }

        public void StopServer()
        {
            _networkRoomManager.gameSceneLoaded -= StartGame;
            _networkRoomManager.allClientsDisconnected -= TryReturnToRoom;
        }

        private void TryReturnToRoom()
        {
            if (NetworkManager.networkSceneName == "Assets/Scenes/Room.unity") return;

            if (NetworkServer.connections.Count == 0) _networkRoomManager.LoadRoom();
        }

        private void StartGame()
        {
            _mapGenerator = FindObjectOfType<MapGenerator>();
            _mapGenerator.Initialize();
            _mapGenerator.Generate();
            NetworkManager.startPositions.Clear();
            NetworkManager.startPositions.Add(FindObjectOfType<Spawnpoint>().transform);
        }
    }
}