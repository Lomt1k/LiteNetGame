using LiteNetLib;
using LiteNetLib.Utils;
using Networking.Client;
using Networking.Connections.Server.Packets;
using Project.UI.Windows.TextChatWindow;
using Project.Scenes;
using Project.UI.Windows;
using Project.UI.Windows.PlayersTabWindow;

namespace Networking.Connections.Client
{
    public static class ClientReceiving_Connections
    {
        private static ClientPlayers _players;
        
        public static void SubscribeToReceivedPackets(NetPacketProcessor packetProcessor)
        {
            packetProcessor.SubscribeReusable<AfterJoinInfoPacket, NetPeer>(OnAfterJoinInfoReceived);
            packetProcessor.SubscribeReusable<ServerAnotherPlayerJoined, NetPeer>(OnAnotherPlayerJoined);
            packetProcessor.SubscribeReusable<ServerAnotherPlayerLeft, NetPeer>(OnAnotherPlayerLeft);
            packetProcessor.SubscribeReusable<PlayersPingInfoPacket, NetPeer>(OnUpdatePlayersPingInfo);
        }
        
        private static void OnAfterJoinInfoReceived(AfterJoinInfoPacket packet, NetPeer peer)
        {
            GameClient.instance.CreatePlayersList(packet.maxPlayers);
            _players = GameClient.instance.players;
            
            foreach (var playerData in packet.playersData)
            {
                bool isMine = playerData.playerId == packet.minePlayerId;
                _players.CreatePlayer(playerData, isMine);
            }
            
            SceneLoader.instance.LoadScene(SceneType.GameWorld, () =>
            {
                //--- on loading
                WindowsManager.CreateWindow<PlayersTabWindow>();
            },() =>
            {
                //--- on loading ends
                TextChatWindow.instance.AddMessage($"Joined to server as <color=#AFAFAF>{NetInfo.minePlayer.nickname}</color>. Players online: {packet.playersData.Length}");
                Project.Units.Client.ClientSending_Units.RequestCreateMineUnit();
            });
        }

        private static void OnAnotherPlayerJoined(ServerAnotherPlayerJoined packet, NetPeer peer)
        {
            _players?.CreatePlayer(packet.PlayerConnectionData, false);
            TextChatWindow.instance.AddMessage($"<color=#AFAFAF>{packet.PlayerConnectionData.nickname} has joined the server");
            PlayersTabWindow.instance?.AddPlayerToTab(packet.PlayerConnectionData.playerId);
        }
        
        private static void OnAnotherPlayerLeft(ServerAnotherPlayerLeft packet, NetPeer peer)
        {
            var player = _players?[packet.playerId];
            if (player == null)
                return;
            
            _players.RemovePlayer(packet.playerId);
            TextChatWindow.instance.AddMessage($"<color=#AFAFAF>{player.nickname} has left the server");
            PlayersTabWindow.instance?.RemovePlayerFromTab(packet.playerId);
        }

        private static void OnUpdatePlayersPingInfo(PlayersPingInfoPacket packet, NetPeer peer)
        {
            foreach (var info in packet.playersPingInfo)
            {
                _players[info.playerId]?.UpdatePing(info.ping);
            }
        }
        
        
    }
}

