using LiteNetLib;

namespace Networking.Server
{
    public class ServerPlayers
    {
        private readonly ServerPlayer[] _players; //index 0 used for "incorrect playerId"

        public int playersCount { get; private set; }

        public ServerPlayers(int maxPlayers)
        {
            _players = new ServerPlayer[maxPlayers + 1];
        }

        public ServerPlayer this[int index] => _players[index];

        public ServerPlayer CreatePlayer(NetPeer peer, string nickname)
        {
            var serverPlayer = new ServerPlayer(peer, nickname);
            var playerId = peer.Id;
            _players[playerId] = serverPlayer;
            playersCount++;

            return serverPlayer;
        }

        public void RemovePlayer(NetPeer peer)
        {
            int playerId = peer.Id;
            if (_players[playerId] == null)
                return;

            _players[playerId] = null;
            playersCount--;
        }
        
        
    }
}

