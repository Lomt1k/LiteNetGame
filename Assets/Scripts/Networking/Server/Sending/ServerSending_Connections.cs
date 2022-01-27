using LiteNetLib;

namespace Networking.Server.Sending
{
    public static class ServerSending_Connections
    {
        private static ServerPacketSender sender => GameServer.instance.sender;
        private static ServerPlayers players => GameServer.instance.players;

        public static void SendAfterJoinServerInfo(ServerPlayer player)
        {
            var dataArray = new SendablePlayerData[players.playersOnline];
            int dataArrayIndex = 0;
            for (int i = 0; i < players.maxPlayers; i++)
            {
                var severPlayer = players[i];
                if (players[i] == null)
                    continue;
                
                dataArray[dataArrayIndex] = new SendablePlayerData()
                {
                    playerId = (ushort) severPlayer.playerId,
                    nickname = severPlayer.nickname
                };
                    
                dataArrayIndex++;
                if (dataArrayIndex == players.playersOnline)
                    break;
            }
                
            var packet = new AfterJoinInfoPacket()
            {
                maxPlayers = (ushort)players.maxPlayers,
                minePlayerId = (ushort)player.playerId,
                playersData = dataArray
            };
            
            sender.SendPacket(player, packet, DeliveryMethod.ReliableOrdered);
        }
        
        

    }
}
