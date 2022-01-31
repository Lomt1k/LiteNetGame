
namespace Networking.Client
{
    public class ClientPlayer : BasePlayer
    {
        public ClientPlayer(PlayerConnectionData playerConnectionData)
        {
            this.playerId = playerConnectionData.playerId;
            this.nickname = playerConnectionData.nickname;
        }
    }
}
