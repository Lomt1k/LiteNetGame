using LiteNetLib;
using LiteNetLib.Utils;
using Project.UI.Windows.TextChatWindow;
using UnityEngine;

namespace Networking.Client.Receiving
{
    public static class ClientReceiving_Connections
    {
        private static ClientPlayers _players;
        
        public static void SubscribeToReceivedPackets(NetPacketProcessor packetProcessor)
        {
            packetProcessor.SubscribeReusable<AfterJoinInfoPacket, NetPeer>(OnAfterJoinInfoReceived);
        }
        
        private static void OnAfterJoinInfoReceived(AfterJoinInfoPacket packet, NetPeer peer)
        {
            GameClient.instance.CreatePlayersList(packet.maxPlayers);
            _players = GameClient.instance.players;
            
            Debug.Log($"ClientReceiving :: OnAfterJoinInfoReceived " +
                      $"| minePlayerId: {packet.minePlayerId} " +
                      $"| playersOnline: {packet.playersData.Length} " +
                      $"| maxPlayers: {packet.maxPlayers}");
            
            foreach (var playerData in packet.playersData)
            {
                Debug.Log($"ClientReceiving :: Player created | {playerData.nickname} (ID {playerData.playerId})");
                bool isMine = playerData.playerId == packet.minePlayerId;
                _players.CreatePlayer(playerData, isMine);
            }
            
            TextChatWindow.instance.AddMessage($"Joined to server. Players online: {packet.playersData.Length}");
        }
        
        
    }
}

