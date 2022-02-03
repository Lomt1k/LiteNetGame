namespace Networking.Client.Sending.Packets.Connections
{
    public class JoinToServerPacket : ClientPacket
    {
        public string nickname { get; set; }
    }

    public class RequestPlayerPingsPacket : ClientPacket { }
    
    
}
