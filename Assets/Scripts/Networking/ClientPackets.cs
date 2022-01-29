
namespace Networking
{
    public abstract class ClientPacket { }
    
    //Auto serializable packets
    public class JoinToServerPacket : ClientPacket
    {
        public string nickname { get; set; }
    }
    
    public class ClientTextChatMessagePacket : ClientPacket
    {
        public string text { get; set; }
    }
    
    
}

