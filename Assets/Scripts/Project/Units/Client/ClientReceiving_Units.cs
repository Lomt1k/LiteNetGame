using LiteNetLib;
using LiteNetLib.Utils;
using Networking;
using Networking.Client;
using Project.Units.Server.Packets;
using UnityEngine;

namespace Project.Units.Client
{
    public static class ClientReceiving_Units
    {
        private const string mineUnitPrefabPath = "Prefabs/Units/MineClientUnit";
        
        private static ClientPlayers players => GameClient.instance.players;
        
        public static void SubscribeToReceivedPackets(NetPacketProcessor packetProcessor)
        {
            packetProcessor.SubscribeReusable<CreateContollableUnitPacket, NetPeer>(CreateLocalControllableUnit);
        }

        public static void CreateLocalControllableUnit(CreateContollableUnitPacket packet, NetPeer peer)
        {
            Debug.Log("CreateLocalControllableUnit");
            var prefab = Resources.Load<GameObject>(mineUnitPrefabPath);
            var unit = Object.Instantiate(prefab, packet.position, packet.rotation).GetComponent<ClientUnit>();
            var minePlayer = NetInfo.minePlayer;
            unit.Initialize(minePlayer);
            minePlayer.SetupUnit(unit);
        }
        
    }
}

