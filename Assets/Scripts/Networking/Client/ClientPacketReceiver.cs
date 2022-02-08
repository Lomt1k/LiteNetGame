using LiteNetLib;
using LiteNetLib.Utils;

namespace Networking.Client
{
    using Receiving;
    
    public class ClientPacketReceiver
    {
        private readonly NetPacketProcessor _packetProcessor;
        
        public ClientPacketReceiver(NetPacketProcessor packetProcessor)
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
            ClientReceiving_Connections.SubscribeToReceivedPackets(_packetProcessor);
            ClientReceiving_TextChat.SubscribeToReceivedPackets(_packetProcessor);
            
            Project.Units.Client.ClientReceiving_Units.SubscribeToReceivedPackets(_packetProcessor);
        }
        
        
        
    }
}

