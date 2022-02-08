using LiteNetLib;
using LiteNetLib.Utils;
using Networking.Server;
using Project.Units.Client.Packets;

namespace Project.Units.Server
{
    public static class ServerReceiving_Units
    {
        private static ServerPlayers players => GameServer.instance.players;
        private static ServerUnitsManager unitsManager => GameServer.instance.unitsManager;
        
        public static void SubscribeToReceivedPackets(NetPacketProcessor packetProcessor)
        {
            packetProcessor.SubscribeReusable<RequestCreateMineUnitPacket, NetPeer>(OnRequestCreateUnit);
        }

        private static void OnRequestCreateUnit(RequestCreateMineUnitPacket packet, NetPeer peer)
        {
            var sender = players[peer.Id];
            if (sender == null)
                return;
            if (sender.unit != null)
                return; //запрос на создание юнита, когда юнит для игрока уже создан
            
            unitsManager.CreateUnitForPlayer(sender);
        }
        

    }
}
