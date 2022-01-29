using LiteNetLib;
using LiteNetLib.Utils;
using UnityEngine;

namespace Networking.Server.Receiving
{
    public static class ServerReceiving_Connections
    {
        private static ServerPlayers players => GameServer.instance.players;
        
        public static void SubscribeToReceivedPackets(NetPacketProcessor packetProcessor)
        {
            packetProcessor.SubscribeReusable<JoinToServerPacket, NetPeer>(OnPlayerJoined);
        }
        
        private static void OnPlayerJoined(JoinToServerPacket packet, NetPeer peer)
        {
            if (players[peer.Id] != null)
                return; //TODO по какой-то причине сообщение о присоединении пришло повторно: скорее всего тут надо разорвать соединение, но хотя бы return;
            
            Debug.Log($"ServerReceiving :: OnPlayerJoinedToServer {packet.nickname} (ID {peer.Id})");
            var newPlayer = GameServer.instance.players.CreatePlayer(peer, packet.nickname);
            Sending.ServerSending_Connections.SendAfterJoinServerInfo(newPlayer);
            
            Sending.ServerSending_Connections.SendAnotherPlayerJoined(newPlayer);
        }
    }
}

