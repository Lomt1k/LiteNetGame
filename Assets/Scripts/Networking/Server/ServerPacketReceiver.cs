using LiteNetLib;
using LiteNetLib.Utils;
using UnityEngine;

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
            _packetProcessor.SubscribeReusable<JoinToServerPacket, NetPeer>(OnPlayerJoined);
        }
        
        private void OnPlayerJoined(JoinToServerPacket packet, NetPeer peer)
        {
            Debug.Log($"{GetType().Name} | OnPlayerJoinedToServer {packet.nickname} (ID {peer.Id})");
            var newPlayer = GameServer.instance.players.CreatePlayer(peer, packet.nickname);
            Sending.Connections.SendAfterJoinServerInfo(newPlayer);
        }
        
    }
}

