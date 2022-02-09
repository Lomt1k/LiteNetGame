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
        private const string anotherClientUnitPrefabPath = "Prefabs/Units/AnotherClientUnit";
        
        private static ClientPlayers players => GameClient.instance.players;
        private static ClientUnit mineUnit;

        private static GameObject _anotherUnitPrefab;

        private static GameObject GetAnotherUnitPrefab()
        {
            if (_anotherUnitPrefab == null)
            {
                _anotherUnitPrefab = Resources.Load<GameObject>(anotherClientUnitPrefabPath);
            }
            return _anotherUnitPrefab;
        }
        
        
        public static void SubscribeToReceivedPackets(NetPacketProcessor packetProcessor)
        {
            packetProcessor.SubscribeReusable<CreateContollableUnitPacket, NetPeer>(CreateLocalControllableUnit);
            packetProcessor.SubscribeReusable<AddObservingUnitPacket, NetPeer>(AddObservingUnit);
            packetProcessor.SubscribeReusable<RemoveObservingUnitPacket, NetPeer>(RemoveObservingUnit);
        }

        public static void CreateLocalControllableUnit(CreateContollableUnitPacket packet, NetPeer peer)
        {
            var prefab = Resources.Load<GameObject>(mineUnitPrefabPath);
            mineUnit = Object.Instantiate(prefab, packet.position, packet.rotation).GetComponent<ClientUnit>();
            var minePlayer = NetInfo.minePlayer;
            mineUnit.Initialize(minePlayer);
            minePlayer.SetupUnit(mineUnit);
        }

        public static void AddObservingUnit(AddObservingUnitPacket packet, NetPeer peer)
        {
            var prefab = GetAnotherUnitPrefab();
            var unit = Object.Instantiate(prefab, packet.position, packet.rotation).GetComponent<ClientUnit>();
            var unitOwner = players[packet.playerId];
            unit.Initialize(unitOwner);
            unitOwner.SetupUnit(unit);
        }

        public static void RemoveObservingUnit(RemoveObservingUnitPacket packet, NetPeer peer)
        {
            var player = players[packet.playerId];
            var unit = player.unit;
            Object.Destroy(unit);
        }
        
    }
}

