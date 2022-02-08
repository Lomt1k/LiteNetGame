
using Project.UI.Windows.PlayersTabWindow;
using Project.Units.Client;

namespace Networking.Client
{
    public sealed class ClientPlayer : BasePlayer
    {
        public ClientUnit unit { get; private set; }
        
        public ClientPlayer(PlayerConnectionData playerConnectionData)
        {
            this.playerId = playerConnectionData.playerId;
            this.ping = playerConnectionData.ping;
            this.nickname = playerConnectionData.nickname;
        }

        public void UpdatePing(int latency)
        {
            this.ping = latency;
            PlayersTabWindow.instance?.RefreshPing(this);
        }
        
        public void SetupUnit(ClientUnit unit)
        {
            this.unit = unit;
        }
        
        
    }
}
