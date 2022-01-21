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
        
        
        
    }
}

