using LiteNetLib;
using Networking.Client;

namespace Project.Units.Client
{
    using Packets;
    
    public static class ClientSending_Units
    {
        private static ClientPacketSender sender => GameClient.instance.sender;
        private static ClientPlayers players => GameClient.instance.players;
        
        public static void RequestCreateMineUnit()
        {
            sender.SendPacket(new RequestCreateMineUnitPacket(), DeliveryMethod.ReliableOrdered);
        }
        
        
        
    }
}

