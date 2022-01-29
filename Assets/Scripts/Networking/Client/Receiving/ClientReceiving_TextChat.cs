using LiteNetLib;
using LiteNetLib.Utils;
using Networking;
using Networking.Client;
using Project.UI.Windows.TextChatWindow;
using UnityEngine;

public static class ClientReceiving_TextChat
{
    private static ClientPlayers _players => GameClient.instance.players;
        
    public static void SubscribeToReceivedPackets(NetPacketProcessor packetProcessor)
    {
        packetProcessor.SubscribeReusable<ServerTextChatMessagePacket, NetPeer>(OnTextChat);
    }
        
    private static void OnTextChat(ServerTextChatMessagePacket packet, NetPeer peer)
    {
        Debug.Log($"ClientReceiving :: OnTextChat {packet.text}");
        TextChatWindow.instance?.AddMessage(packet.text);
    }
    
}
