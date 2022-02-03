using LiteNetLib;

namespace Networking.Client.Sending
{
    using Packets.Connections;
    
    public static class ClientSending_Connections
    {
        private static ClientPacketSender sender => GameClient.instance.sender;
        private static ClientPlayers players => GameClient.instance.players;
        
        public static void SendJoinToServer(string nickname)
        {
            sender.SendPacket(new JoinToServerPacket {nickname = nickname}, DeliveryMethod.ReliableOrdered);
        }

        public static void RequestPlayerPings()
        {
            sender.SendPacket(new RequestPlayerPingsPacket(), DeliveryMethod.Unreliable);
        }
        
    }
}

