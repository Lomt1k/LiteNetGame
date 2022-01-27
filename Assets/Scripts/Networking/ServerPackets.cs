
namespace Networking
{
    public abstract class ServerPacket { }
    
    public class AfterJoinInfoPacket : ServerPacket
    {
        public ushort maxPlayers { get; set; }
        public ushort minePlayerId { get; set; }
        public SendablePlayerData[] playersData { get; set; }
    }
}