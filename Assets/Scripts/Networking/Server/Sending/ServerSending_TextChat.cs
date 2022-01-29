using LiteNetLib;

namespace Networking.Server.Sending
{
    public static class ServerSending_TextChat
    {
        private static ServerPacketSender sender => GameServer.instance.sender;
        private static ServerPlayers players => GameServer.instance.players;
        
        public static void SendTextChatMessage(ServerPlayer targetPlayer, string text)
        {
            sender.SendPacket(targetPlayer, new ServerTextChatMessagePacket() {text = text}, DeliveryMethod.ReliableOrdered);
        }
        
        public static void SendTextChatMessageToAll(string text)
        {
            sender.SendPacketToAll(new ServerTextChatMessagePacket() {text = text}, DeliveryMethod.ReliableOrdered);
        }
        
        
    }
}