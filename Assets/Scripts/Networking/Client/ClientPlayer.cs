
namespace Networking.Client
{
    public class ClientPlayer : BasePlayer
    {
        public ClientPlayer(SendablePlayerData playerData)
        {
            this.playerId = playerData.playerId;
            this.nickname = playerData.nickname;
        }
    }
}
