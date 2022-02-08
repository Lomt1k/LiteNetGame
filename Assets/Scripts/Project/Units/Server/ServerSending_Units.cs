using LiteNetLib;
using Networking.Server;
using UnityEngine;

namespace Project.Units.Server
{
    using Packets;
    
    public static class ServerSending_Units
    {
        private static ServerPacketSender sender => GameServer.instance.sender;
        private static ServerPlayers players => GameServer.instance.players;
        
        public static void SendCreateControllableUnitForPlayer(ServerPlayer player, Vector3 position, Quaternion rotation)
        {
            var packet = new CreateContollableUnitPacket
            {
                position = position,
                rotation = rotation
            };
            sender.SendPacket(player, packet, DeliveryMethod.ReliableOrdered);
        }
        
        
        
    }
}
