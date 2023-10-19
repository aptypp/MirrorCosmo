using Mirror;

namespace MomoCoop
{
    public sealed class GameNetworkRoomPlayer : NetworkRoomPlayer
    {
        public override void Start()
        {
            base.Start();

            if (!isLocalPlayer) return;

            GameClient gameClient = FindObjectOfType<GameClient>();
            
            gameClient.SetNetworkRoomPlayer(this);
        }
    }
}