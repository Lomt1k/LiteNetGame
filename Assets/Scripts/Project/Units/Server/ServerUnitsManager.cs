using Networking.Server;
using Networking.Server.Observing;
using UnityEngine;

namespace Project.Units.Server
{
    public class ServerUnitsManager
    {
        private const string unitPrefabPath = "Prefabs/Units/ServerUnit";

        private readonly GameObject unitPrefab;
        
        private static ServerPlayers players => GameServer.instance.players;
        private static ObservationManager observationManager => GameServer.instance.observationManager;
        
        private readonly ServerUnit[] _units;

        public ServerUnitsManager(int maxPlayers)
        {
            _units = new ServerUnit[maxPlayers];
            unitPrefab = Resources.Load<GameObject>(unitPrefabPath);
        }

        public void CreateUnitForPlayer(ServerPlayer player)
        {
            //TODO установить position и rotation
            var position = Vector3.zero;
            var rotation = Quaternion.identity;

            var unit = Object.Instantiate(unitPrefab, position, rotation).GetComponent<ServerUnit>();
            unit.Initialize(player);
            player.SetupUnit(unit);

            _units[player.playerId] = unit;
            observationManager.RegisterUnit(unit);
            ServerSending_Units.SendCreateControllableUnitForPlayer(player, position, rotation);
        }

        public void DestroyPlayerUnit(ServerUnit unit)
        {
            observationManager.UnregisterUnit(unit);
            _units[unit.player.playerId] = null;
            Object.Destroy(unit.gameObject);
        }
        
        
    }
}

