using LiteNetLib;
using LiteNetLib.Utils;
using UnityEngine;

namespace Networking.Server.Receiving
{
    public static class ServerReceiving_Connections
    {
        public static void SubscribeToReceivedPackets(NetPacketProcessor packetProcessor)
        {
            packetProcessor.SubscribeReusable<JoinToServerPacket, NetPeer>(OnPlayerJoined);
        }
        
        private static void OnPlayerJoined(JoinToServerPacket packet, NetPeer peer)
        {
            Debug.Log($"ServerReceiving :: OnPlayerJoinedToServer {packet.nickname} (ID {peer.Id})");
            var newPlayer = GameServer.instance.players.CreatePlayer(peer, packet.nickname);
            Sending.ServerSending_Connections.SendAfterJoinServerInfo(newPlayer);
        }
    }
}

