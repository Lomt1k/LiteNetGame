using LiteNetLib;

namespace Networking.Server
{
    public class ServerPlayers
    {
        private readonly ServerPlayer[] _players;

        public int playersOnline { get; private set; }
        public int maxPlayers => _players.Length;

        public ServerPlayers(int maxPlayers)
        {
            _players = new ServerPlayer[maxPlayers];
        }

        public ServerPlayer this[int index] => _players[index];

        public ServerPlayer CreatePlayer(NetPeer peer, string nickname)
        {
            var serverPlayer = new ServerPlayer(peer, nickname);
            var playerId = peer.Id;
            _players[playerId] = serverPlayer;
            playersOnline++;

            return serverPlayer;
        }

        public void RemovePlayer(NetPeer peer)
        {
            int playerId = peer.Id;
            if (_players[playerId] == null)
                return;

            _players[playerId].OnDisconnect();
            _players[playerId] = null;
            playersOnline--;
        }
        
        
    }
}

