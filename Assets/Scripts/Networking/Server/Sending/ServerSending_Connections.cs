using LiteNetLib;

namespace Networking.Server.Sending
{
    using Packets.Connections;
    
    public static class ServerSending_Connections
    {
        private static ServerPacketSender sender => GameServer.instance.sender;
        private static ServerPlayers players => GameServer.instance.players;

        public static void SendInfoAboutAllConnections(ServerPlayer targetPlayer)
        {
            var dataArray = new PlayerConnectionData[players.playersOnline];
            int dataArrayIndex = 0;
            for (int i = 0; i < players.maxPlayers; i++)
            {
                var serverPlayer = players[i];
                if (serverPlayer == null)
                    continue;
                
                dataArray[dataArrayIndex] = new PlayerConnectionData
                {
                    playerId = (ushort) serverPlayer.playerId,
                    ping = (ushort) serverPlayer.peer.Ping,
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

        public static void SendNewConnectionInfoToAll(ServerPlayer newPlayer)
        {
            var playerData = new PlayerConnectionData
            {
                playerId = (ushort) newPlayer.playerId,
                ping = (ushort) newPlayer.peer.Ping,
                nickname = newPlayer.nickname
            };
            var packet = new ServerAnotherPlayerJoined {PlayerConnectionData = playerData};
            sender.SendPacketToAll(packet, DeliveryMethod.ReliableOrdered, newPlayer);
        }
        
        public static void SendPlayerDisconnectInfoToAll(ServerPlayer disconnectedPlayer)
        {
            var packet = new ServerAnotherPlayerLeft {playerId = (ushort) disconnectedPlayer.playerId};
            sender.SendPacketToAll(packet, DeliveryMethod.ReliableOrdered, disconnectedPlayer);
        }

        public static void SendAllPlayersPingInfo(ServerPlayer targetPlayer)
        {
            var dataArray = new PlayerPingInfo[players.playersOnline];
            int dataArrayIndex = 0;
            for (int i = 0; i < players.maxPlayers; i++)
            {
                var serverPlayer = players[i];
                if (serverPlayer == null)
                    continue;
                
                dataArray[dataArrayIndex] = new PlayerPingInfo
                {
                    playerId = (ushort) serverPlayer.playerId,
                    ping = (ushort) serverPlayer.peer.Ping
                };
                    
                dataArrayIndex++;
                if (dataArrayIndex == players.playersOnline)
                    break;
            }
                
            var packet = new PlayersPingInfoPacket
            {
                playersPingInfo = dataArray
            };
            
            sender.SendPacket(targetPlayer, packet, DeliveryMethod.Unreliable);
        }
        
        
        

    }
}
