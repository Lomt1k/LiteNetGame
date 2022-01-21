namespace Networking
{
    public abstract class ServerPacket { }
    
    public class TestPacket : ServerPacket
    {
        public string nickname { get; set; }
    }
}