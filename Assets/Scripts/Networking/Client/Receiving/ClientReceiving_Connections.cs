using LiteNetLib;
using LiteNetLib.Utils;
using Project.UI.Windows.TextChatWindow;
using UnityEngine;
using Networking.Server.Sending.Packets.Connections;
using Project.UI.Windows;
using Project.UI.Windows.PlayersTabWindow;

namespace Networking.Client.Receiving
{
    public static class ClientReceiving_Connections
    {
        private static ClientPlayers _players;
        
        public static void SubscribeToReceivedPackets(NetPacketProcessor packetProcessor)
        {
            packetProcessor.SubscribeReusable<AfterJoinInfoPacket, NetPeer>(OnAfterJoinInfoReceived);
            packetProcessor.SubscribeReusable<ServerAnotherPlayerJoined, NetPeer>(OnAnotherPlayerJoined);
            packetProcessor.SubscribeReusable<ServerAnotherPlayerLeft, NetPeer>(OnAnotherPlayerLeft);
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
                Debug.Log($"ClientReceiving :: Player created | [ID {playerData.playerId}] {playerData.nickname} (Ping {playerData.ping})");
                bool isMine = playerData.playerId == packet.minePlayerId;
                _players.CreatePlayer(playerData, isMine);
            }

            WindowsManager.CreateWindow<Project.UI.Windows.PlayersTabWindow.PlayersTabWindow>();
            TextChatWindow.instance.AddMessage($"Joined to server as <color=#AFAFAF>{NetInfo.minePlayer.nickname}</color>. Players online: {packet.playersData.Length}");
        }

        private static void OnAnotherPlayerJoined(ServerAnotherPlayerJoined packet, NetPeer peer)
        {
            _players?.CreatePlayer(packet.PlayerConnectionData, false);
            TextChatWindow.instance.AddMessage($"<color=#AFAFAF>{packet.PlayerConnectionData.nickname} has joined the server");
            PlayersTabWindow.instance.AddPlayerToTab(packet.PlayerConnectionData.playerId);
        }
        
        private static void OnAnotherPlayerLeft(ServerAnotherPlayerLeft packet, NetPeer peer)
        {
            var player = _players?[packet.playerId];
            if (player == null)
                return;
            
            _players.RemovePlayer(packet.playerId);
            TextChatWindow.instance.AddMessage($"<color=#AFAFAF>{player.nickname} has left the server");
            PlayersTabWindow.instance.RemovePlayerFromTab(packet.playerId);
        }
        
        
    }
}

