using LiteNetLib;

namespace Networking.Server
{
    public class ServerPlayer : BasePlayer
    {
        public NetPeer peer { get; }
        public override int ping => peer.Ping;

        public ServerPlayer(NetPeer peer, string nickname)
        {
            this.playerId = peer.Id;
            this.peer = peer;
            this.nickname = nickname;
        }

        
    }
}