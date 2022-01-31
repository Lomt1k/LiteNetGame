using LiteNetLib;
using LiteNetLib.Utils;
using Project.UI.Windows.TextChatWindow;
using Networking.Server.Sending.Packets.TextChat;

namespace Networking.Client.Receiving
{
    public static class ClientReceiving_TextChat
    {
        private static ClientPlayers _players => GameClient.instance.players;
        
        public static void SubscribeToReceivedPackets(NetPacketProcessor packetProcessor)
        {
            packetProcessor.SubscribeReusable<ServerTextChatMessagePacket, NetPeer>(OnTextChat);
        }
        
        private static void OnTextChat(ServerTextChatMessagePacket packet, NetPeer peer)
        {
            TextChatWindow.instance?.AddMessage(packet.text);
        }
    
    }
}

