using LiteNetLib;

namespace Networking.Server
{
    public class ServerPlayer : BasePlayer
    {
        public NetPeer peer { get; }
        
        public ServerPlayer(NetPeer peer, string nickname)
        {
            this.playerId = peer.Id;
            this.peer = peer;
            this.nickname = nickname;
        }
    }
}