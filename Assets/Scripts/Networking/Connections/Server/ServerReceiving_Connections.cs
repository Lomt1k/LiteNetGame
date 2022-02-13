using LiteNetLib;
using LiteNetLib.Utils;
using UnityEngine;
using Networking.Server;
using Networking.Connections.Client.Packets;

namespace Networking.Connections.Server
{
    public static class ServerReceiving_Connections
    {
        private static ServerPlayers players => GameServer.instance.players;
        
        public static void SubscribeToReceivedPackets(NetPacketProcessor packetProcessor)
        {
            packetProcessor.SubscribeReusable<JoinToServerPacket, NetPeer>(OnPlayerJoined);
            packetProcessor.SubscribeReusable<RequestPlayerPingsPacket, NetPeer>(OnRequestPlayerPings);
        }
        
        private static void OnPlayerJoined(JoinToServerPacket packet, NetPeer peer)
        {
            if (players[peer.Id] != null)
                return; //TODO по какой-то причине сообщение о присоединении пришло повторно: скорее всего тут надо разорвать соединение, но хотя бы нужен return;
            
            Debug.Log($"ServerReceiving :: OnPlayerJoinedToServer {packet.nickname} (ID {peer.Id})");
            var newPlayer = GameServer.instance.players.CreatePlayer(peer, packet.nickname);
            ServerSending_Connections.SendInfoAboutAllConnections(newPlayer);
            
            ServerSending_Connections.SendNewConnectionInfoToAll(newPlayer);
        }

        private static void OnRequestPlayerPings(RequestPlayerPingsPacket packet, NetPeer peer)
        {
            var senderPlayer = players[peer.Id];
            if (senderPlayer == null)
                return;
            
            ServerSending_Connections.SendAllPlayersPingInfo(senderPlayer);
        }
        
        
        
    }
}

