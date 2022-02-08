using System.Collections;
using System.Collections.Generic;
using Networking.Client;
using Networking.Server;
using UnityEngine;

namespace Project.Units.Server
{
    public class ServerUnitsManager
    {
        private const string unitPrefabPath = "Prefabs/Units/ServerUnit";

        private readonly GameObject unitPrefab;
        
        private static ServerPlayers players => GameServer.instance.players;
        
        private ServerUnit[] _units;

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
            
            ServerSending_Units.SendCreateControllableUnitForPlayer(player, position, rotation);
        }
        
        
    }
}

