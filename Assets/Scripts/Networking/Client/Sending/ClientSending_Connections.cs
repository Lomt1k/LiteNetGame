using LiteNetLib;

namespace Networking.Client.Sending
{
    public static class ClientSending_Connections
    {
        private static ClientPacketSender sender => GameClient.instance.sender;
        private static ClientPlayers players => GameClient.instance.players;

        public static void SendJoinServerPacket()
        {
            sender.SendPacket(new JoinToServerPacket {nickname = "Lomt1k"}, DeliveryMethod.ReliableOrdered);
        }
        
    }
}

