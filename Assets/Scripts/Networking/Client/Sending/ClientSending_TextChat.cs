using LiteNetLib;

namespace Networking.Client.Sending
{
    public static class ClientSending_TextChat
    {
        private static ClientPacketSender sender => GameClient.instance.sender;
        private static ClientPlayers players => GameClient.instance.players;

        public static void SendTextChatMessage(string text)
        {
            sender.SendPacket(new ClientTextChatMessagePacket() {text = text}, DeliveryMethod.ReliableOrdered);
        }
        
    }
}

