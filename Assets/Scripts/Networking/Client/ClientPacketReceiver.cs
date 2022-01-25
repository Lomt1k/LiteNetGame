using LiteNetLib;
using LiteNetLib.Utils;
using UnityEngine;

namespace Networking.Client
{
    public class ClientPacketReceiver
    {
        private readonly NetPacketProcessor _packetProcessor;
        private ClientPlayers _players;
        
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
            _packetProcessor.SubscribeReusable<AfterJoinInfoPacket, NetPeer>(OnAfterJoinInfoReceived);
        }
        
        private void OnAfterJoinInfoReceived(AfterJoinInfoPacket packet, NetPeer peer)
        {
            GameClient.instance.CreatePlayersList(packet.maxPlayers);
            _players = GameClient.instance.players;
            
            Debug.Log($"{GetType().Name} | OnAfterJoinInfoReceived " +
                      $"| minePlayerId: {packet.minePlayerId} " +
                      //$"| playersOnline: {packet.playersData.Length} " +
                      $"| maxPlayers: {packet.maxPlayers}");
            
            //Debug.Log($"Test read data {packet.testData.playerId} {packet.testData.nickname}");
            Debug.Log($"Test vector3 {packet.vector3.ToString()}");
            
            // foreach (var playerData in packet.playersData)
            // {
            //     Debug.Log($"Player created | {playerData.nickname} (ID {playerData.playerId})");
            //     _players.CreatePlayer(playerData);
            // }
        }
        
    }
}

