using System;
using Mirror;

namespace MomoCoop.NetworkMessages
{
    public struct SpawnPlayersMessage : NetworkMessage { }

    public struct ReadyToSpawnMessage : NetworkMessage
    {
        public int connectionId;
        public ArraySegment<int> connectionsIds;

        public ReadyToSpawnMessage(int connectionId, ArraySegment<int> connectionsIds)
        {
            this.connectionId = connectionId;
            this.connectionsIds = connectionsIds;
        }
    }
}