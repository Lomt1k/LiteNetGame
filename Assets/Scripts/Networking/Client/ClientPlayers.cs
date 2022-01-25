
namespace Networking.Client
{
    public class ClientPlayers
    {
        private readonly ClientPlayer[] _players; //index 0 used for "incorrect playerId"
        
        public int playersOnline { get; private set; }
        public int maxPlayers => _players.Length;

        public ClientPlayers(int maxPlayers)
        {
            _players = new ClientPlayer[maxPlayers];
        }
        
        public ClientPlayer this[int index] => _players[index];
        
        public ClientPlayer CreatePlayer(SendablePlayerData playerData)
        {
            var clientPlayer = new ClientPlayer(playerData);
            var playerId = clientPlayer.playerId;
            _players[playerId] = clientPlayer;
            playersOnline++;

            return clientPlayer;
        }
        
        public void RemovePlayer(int playerId)
        {
            if (_players[playerId] == null)
                return;

            _players[playerId] = null;
            playersOnline--;
        }
        
    }
}
