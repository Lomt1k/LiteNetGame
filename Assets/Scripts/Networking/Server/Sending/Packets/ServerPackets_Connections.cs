
namespace Networking.Server.Sending.Packets.Connections
{
    
    public class AfterJoinInfoPacket : ServerPacket
    {
        public ushort maxPlayers { get; set; }
        public ushort minePlayerId { get; set; }
        public PlayerConnectionData[] playersData { get; set; }
    }
    
    public class ServerAnotherPlayerJoined : ServerPacket
    {
        public PlayerConnectionData PlayerConnectionData { get; set; }
    }
    
    public class ServerAnotherPlayerLeft : ServerPacket
    {
        public ushort playerId { get; set; }
    }
    
}
