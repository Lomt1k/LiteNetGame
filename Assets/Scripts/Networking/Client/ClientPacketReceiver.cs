﻿using LiteNetLib;
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
        }
        
        
        
    }
}

