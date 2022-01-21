using LiteNetLib;
using LiteNetLib.Utils;

namespace Networking.Client
{
    public class ClientPacketSender
    {
        private readonly NetManager _netManager;
        private readonly NetPacketProcessor _packetProcessor;
        private readonly NetDataWriter _dataWriter;

        public ClientPacketSender(NetManager netManager, NetPacketProcessor packetProcessor)
        {
            _netManager = netManager;
            _packetProcessor = packetProcessor;
            _dataWriter = new NetDataWriter();
        }
        
        public void SendPacket<T>(T packet, DeliveryMethod deliveryMethod) where T : ClientPacket, new()
        {
            _dataWriter.Reset();
            _packetProcessor.Write(_dataWriter, packet);
            _netManager.FirstPeer.Send(_dataWriter, deliveryMethod);
        }
        
        
        
    }
}

