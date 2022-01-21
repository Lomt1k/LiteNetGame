using LiteNetLib;
using LiteNetLib.Utils;

namespace Networking.Client
{
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
            //_packetProcessor.SubscribeReusable<JoinPacket, NetPeer>(OnJoinReceived);
        }
        
        // private void OnJoinReceived(JoinPacket joinPacket, NetPeer peer)
        // {
        //     Debug.Log($"{GetType()} | OnJoinReceived {joinPacket.nickname} (ID {peer.Id})");
        // }
        
    }
}

