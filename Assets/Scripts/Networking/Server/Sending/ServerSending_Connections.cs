using LiteNetLib;

namespace Networking.Server.Sending
{
    public static class ServerSending_Connections
    {
        private static ServerPacketSender sender => GameServer.instance.sender;
        private static ServerPlayers players => GameServer.instance.players;

        public static void SendAfterJoinServerInfo(ServerPlayer targetPlayer)
        {
            var dataArray = new SendablePlayerData[players.playersOnline];
            int dataArrayIndex = 0;
            for (int i = 0; i < players.maxPlayers; i++)
            {
                var serverPlayer = players[i];
                if (players[i] == null)
                    continue;
                
                dataArray[dataArrayIndex] = new SendablePlayerData
                {
                    playerId = (ushort) serverPlayer.playerId,
                    nickname = serverPlayer.nickname
                };
                    
                dataArrayIndex++;
                if (dataArrayIndex == players.playersOnline)
                    break;
            }
                
            var packet = new AfterJoinInfoPacket
            {
                maxPlayers = (ushort)players.maxPlayers,
                minePlayerId = (ushort)targetPlayer.playerId,
                playersData = dataArray
            };
            
            sender.SendPacket(targetPlayer, packet, DeliveryMethod.ReliableOrdered);
        }

        public static void SendAnotherPlayerJoined(ServerPlayer anotherPlayer)
        {
            var playerData = new SendablePlayerData
            {
                playerId = (ushort) anotherPlayer.playerId,
                nickname = anotherPlayer.nickname
            };
            var packet = new ServerAnotherPlayerJoined {playerData = playerData};
            sender.SendPacketToAll(packet, DeliveryMethod.ReliableOrdered, anotherPlayer);
        }
        
        public static void SendAnotherPlayerLeft(ServerPlayer anotherPlayer)
        {
            var packet = new ServerAnotherPlayerLeft {playerId = (ushort) anotherPlayer.playerId};
            sender.SendPacketToAll(packet, DeliveryMethod.ReliableOrdered, anotherPlayer);
        }
        
        

    }
}
