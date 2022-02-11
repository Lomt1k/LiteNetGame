using Networking.Connections.DataTypes;

namespace Networking.Client
{
    public class ClientPlayers
    {
        private readonly ClientPlayer[] _players;
        
        public int playersOnline { get; private set; }
        public int maxPlayers => _players.Length;
        public int minePlayerid = -1;
        public ClientPlayer minePlayer;

        public ClientPlayers(int maxPlayers)
        {
            _players = new ClientPlayer[maxPlayers];
        }
        
        public ClientPlayer this[int index] => _players[index];
        
        public ClientPlayer CreatePlayer(PlayerConnectionData playerConnectionData, bool isMine)
        {
            var clientPlayer = new ClientPlayer(playerConnectionData);
            var playerId = clientPlayer.playerId;
            _players[playerId] = clientPlayer;
            playersOnline++;

            if (isMine)
            {
                minePlayer = clientPlayer;
                minePlayerid = playerId;
            }

            return clientPlayer;
        }
        
        public void RemovePlayer(int playerId)
        {
            if (_players[playerId] == null)
                return;

            _players[playerId].OnDisconnect();
            _players[playerId] = null;
            playersOnline--;
        }
        
    }
}
