using LiteNetLib;
using LiteNetLib.Utils;
using UnityEngine;

namespace Networking.Server.Receiving
{
    public static class ServerReceiving_TextChat
    {
        private static ServerPlayers players => GameServer.instance.players;
        
        public static void SubscribeToReceivedPackets(NetPacketProcessor packetProcessor)
        {
            packetProcessor.SubscribeReusable<ClientTextChatMessagePacket, NetPeer>(OnTextChat);
        }
        
        private static void OnTextChat(ClientTextChatMessagePacket packet, NetPeer peer)
        {
            var sender = players[peer.Id];
            if (sender == null)
                return;

            Debug.Log($"ServerReceiving :: OnTextChat | {sender.nickname} [ID {peer.Id}]: {packet.text}");
            var formattedText = $"<color=#AFAFAF>{sender.nickname}:</color> {packet.text}";
            Sending.ServerSending_TextChat.SendTextChatMessageToAll(formattedText);
        }
        
        
    }
}

