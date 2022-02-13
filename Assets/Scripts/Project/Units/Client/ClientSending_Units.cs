using LiteNetLib;
using Networking.Client;
using Project.Units.DataTypes;
using UnityEngine;

namespace Project.Units.Client
{
    using Packets;
    
    public static class ClientSending_Units
    {
        private static ClientPacketSender sender => GameClient.instance.sender;

        private static ushort lastUnitStateUpdatePacketId;
        
        public static void RequestCreateMineUnit()
        {
            sender.SendPacket(new RequestCreateMineUnitPacket(), DeliveryMethod.ReliableOrdered);
        }

        public static void SendMineUnitStateUpdate(Vector3 position, Quaternion rotation, UnitStateInfo stateInfo)
        {
            lastUnitStateUpdatePacketId++;
            var packet = new UpdateMineUnitStatePacket
            {
                packetId = lastUnitStateUpdatePacketId,
                position = position,
                rotation = rotation,
                stateInfo = stateInfo
            };
            sender.SendPacket(packet, DeliveryMethod.Unreliable);
        }
        
        
        
    }
}

