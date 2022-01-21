
namespace Networking
{
    public abstract class ClientPacket { }
    
    //Auto serializable packets
    public class JoinPacket : ClientPacket
    {
        public string nickname { get; set; }
    }
}

