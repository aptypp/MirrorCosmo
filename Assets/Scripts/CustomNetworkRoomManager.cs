using System;
using Mirror;
using UnityEngine;

namespace MomoCoop
{
    public sealed class CustomNetworkRoomManager : NetworkRoomManager
    {
        public event Action gameSceneLoaded;
        public event Action gameStarted;
        public event Action allClientsDisconnected;

        private const string _ACTION_SCENE_NAME = "Assets/Scenes/Action.unity";

        public override void OnServerSceneChanged(string sceneName)
        {
            if (sceneName == _ACTION_SCENE_NAME)
            {
                gameSceneLoaded?.Invoke();
            }

            base.OnServerSceneChanged(sceneName);

            if (sceneName == _ACTION_SCENE_NAME)
            {
                gameStarted?.Invoke();
            }
        }

        public override void OnRoomServerDisconnect(NetworkConnectionToClient conn) => allClientsDisconnected?.Invoke();

        public void LoadRoom() => ServerChangeScene(RoomScene);

        public void LoadAction() => ServerChangeScene(GameplayScene);
    }
}