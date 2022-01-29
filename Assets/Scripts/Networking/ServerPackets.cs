
namespace Networking
{
    public abstract class ServerPacket { }
    
    public class AfterJoinInfoPacket : ServerPacket
    {
        public ushort maxPlayers { get; set; }
        public ushort minePlayerId { get; set; }
        public SendablePlayerData[] playersData { get; set; }
    }
    
    public class ServerTextChatMessagePacket : ServerPacket
    {
        public string text { get; set; }
    }

    public class ServerAnotherPlayerJoined : ServerPacket
    {
        public SendablePlayerData playerData { get; set; }
    }
    
    public class ServerAnotherPlayerLeft : ServerPacket
    {
        public ushort playerId { get; set; }
    }
    
    
}