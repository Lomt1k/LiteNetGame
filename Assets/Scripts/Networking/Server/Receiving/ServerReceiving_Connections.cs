using LiteNetLib;
using LiteNetLib.Utils;
using UnityEngine;
using Networking.Client.Sending.Packets.Connections;

namespace Networking.Server.Receiving
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
            Sending.ServerSending_Connections.SendInfoAboutAllConnections(newPlayer);
            
            Sending.ServerSending_Connections.SendNewConnectionInfoToAll(newPlayer);
        }

        private static void OnRequestPlayerPings(RequestPlayerPingsPacket packet, NetPeer peer)
        {
            var senderPlayer = players[peer.Id];
            if (senderPlayer == null)
                return;
            
            Sending.ServerSending_Connections.SendAllPlayersPingInfo(senderPlayer);
        }
        
        
        
    }
}

