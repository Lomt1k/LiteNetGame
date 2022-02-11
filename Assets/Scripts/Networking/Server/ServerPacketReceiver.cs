using LiteNetLib;
using LiteNetLib.Utils;
using Networking.Server.Receiving;

namespace Networking.Server
{
    public class ServerPacketReceiver
    {
        private readonly NetPacketProcessor _packetProcessor;
        
        public ServerPacketReceiver(NetPacketProcessor packetProcessor)
        {
            _packetProcessor = packetProcessor;
            SubscribeToReceivedPackets();
        }

        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            _packetProcessor.ReadAllPackets(reader, peer);
        }

        private void SubscribeToReceivedPackets()
        {
            Connections.Server.ServerReceiving_Connections.SubscribeToReceivedPackets(_packetProcessor);
            ServerReceiving_TextChat.SubscribeToReceivedPackets(_packetProcessor);
            
            Project.Units.Server.ServerReceiving_Units.SubscribeToReceivedPackets(_packetProcessor);
        }
        
    }
}

