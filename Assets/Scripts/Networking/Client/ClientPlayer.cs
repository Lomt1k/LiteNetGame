
namespace Networking.Client
{
    public sealed class ClientPlayer : BasePlayer
    {
        
        public ClientPlayer(PlayerConnectionData playerConnectionData)
        {
            this.playerId = playerConnectionData.playerId;
            this.ping = playerConnectionData.ping;
            this.nickname = playerConnectionData.nickname;
        }

        public void UpdatePing(int latency)
        {
            this.ping = latency;
        }
    }
}
