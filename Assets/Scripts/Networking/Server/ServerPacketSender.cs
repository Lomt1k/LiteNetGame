using LiteNetLib;
using LiteNetLib.Utils;

namespace Networking.Server
{
    public class ServerPacketSender
    {
        private readonly NetManager _netManager;
        private readonly NetPacketProcessor _packetProcessor;
        private readonly NetDataWriter _dataWriter;

        public ServerPacketSender(NetManager netManager, NetPacketProcessor packetProcessor)
        {
            _netManager = netManager;
            _packetProcessor = packetProcessor;
            _dataWriter = new NetDataWriter();
        }
        
        public void SendPacket<T>(NetPeer peer, T packet, DeliveryMethod deliveryMethod) where T : ServerPacket, new()
        {
            _dataWriter.Reset();
            _packetProcessor.Write(_dataWriter, packet);
            peer.Send(_dataWriter, deliveryMethod);
        }
        
        public void SendPacket<T>(ServerPlayer player, T packet, DeliveryMethod deliveryMethod) where T : ServerPacket, new()
        {
            var peer = player.peer;
            SendPacket(peer, packet, deliveryMethod);
        }
        
        public void SendPacket<T>(int playerId, T packet, DeliveryMethod deliveryMethod) where T : ServerPacket, new()
        {
            var serverPlayer = GameServer.instance.players[playerId];
            SendPacket(serverPlayer, packet, deliveryMethod);
        }
        
        public void SendPacketToAll<T>(T packet, DeliveryMethod deliveryMethod) where T : ServerPacket, new()
        {
            _dataWriter.Reset();
            _packetProcessor.Write(_dataWriter, packet);
            _netManager.SendToAll(_dataWriter, deliveryMethod);
        }
        
        public void SendPacketToAll<T>(T packet, DeliveryMethod deliveryMethod, NetPeer excludePeer) where T : ServerPacket, new()
        {
            _dataWriter.Reset();
            _packetProcessor.Write(_dataWriter, packet);
            _netManager.SendToAll(_dataWriter, deliveryMethod, excludePeer);
        }
        
        public void SendPacketToAll<T>(T packet, DeliveryMethod deliveryMethod, ServerPlayer excludePlayer) where T : ServerPacket, new()
        {
            var excludePeer = excludePlayer.peer;
            SendPacketToAll(packet, deliveryMethod, excludePeer);
        }
        
        public void SendPacketToAll<T>(T packet, DeliveryMethod deliveryMethod, int excludePlayerId) where T : ServerPacket, new()
        {
            var serverPlayer = GameServer.instance.players[excludePlayerId];
            SendPacketToAll(packet, deliveryMethod, serverPlayer);
        }
        
    }
}

