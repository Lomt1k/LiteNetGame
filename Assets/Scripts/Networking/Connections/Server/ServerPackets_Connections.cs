namespace Networking.Connections.Server.Packets
{
    using DataTypes;
    
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
    
    public class PlayersPingInfoPacket : ServerPacket
    {
        public PlayerPingInfo[] playersPingInfo { get; set; }
    }
    
}
