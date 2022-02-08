using LiteNetLib;
using Project.Units.Server;

namespace Networking.Server
{
    public class ServerPlayer : BasePlayer
    {
        public NetPeer peer { get; }
        public override int ping => peer.Ping;
        public ServerUnit unit { get; private set; }

        public ServerPlayer(NetPeer peer, string nickname)
        {
            this.playerId = peer.Id;
            this.peer = peer;
            this.nickname = nickname;
        }

        public void SetupUnit(ServerUnit unit)
        {
            this.unit = unit;
        }

        
    }
}