namespace Networking.Connections.Client.Packets
{
    public class JoinToServerPacket : ClientPacket
    {
        public string nickname { get; set; }
    }

    public class RequestPlayerPingsPacket : ClientPacket { }
    
    
}
