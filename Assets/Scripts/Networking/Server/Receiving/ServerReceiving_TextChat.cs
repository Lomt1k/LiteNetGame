using System.Linq;
using LiteNetLib;
using LiteNetLib.Utils;
using UnityEngine;
using Networking.Client.Sending.Packets.TextChat;

namespace Networking.Server.Receiving
{
    public static class ServerReceiving_TextChat
    {
        private static ServerPlayers players => GameServer.instance.players;
        
        private const int maxMessageLength = 80;
        
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
            
            //TODO: Не шибко оптимизированная работа со строками
            var formattedText = $"{sender.nickname}: {packet.text}";
            if (formattedText.Length <= maxMessageLength)
            {
                SetColoredNickname(ref formattedText, sender.nickname, "#AFAFAF");
                Sending.ServerSending_TextChat.SendTextChatMessageToAll(formattedText);
            }
            else
            {
                var texts = formattedText.SplitByLength(maxMessageLength).ToArray();
                SetColoredNickname(ref texts[0], sender.nickname, "#AFAFAF");
                foreach (var text in texts)
                {
                    Sending.ServerSending_TextChat.SendTextChatMessageToAll(text);
                }
            }

        }

        private static void SetColoredNickname(ref string formattedText, string nickname, string color)
        {
            formattedText = formattedText.Insert(nickname.Length + 1, "</color>");
            formattedText = formattedText.Insert(0, $"<color={color}>");
        }
        
        
    }
}

