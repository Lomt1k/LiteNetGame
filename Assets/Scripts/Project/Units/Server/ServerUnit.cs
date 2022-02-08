using Networking.Server;
using UnityEngine;

namespace Project.Units.Server
{
    public class ServerUnit : UnitBase
    {
        public ServerPlayer player { get; private set; }
        
        public void Initialize(ServerPlayer ownerPlayer)
        {
            player = ownerPlayer;
            DontDestroyOnLoad(gameObject);
            
            #if UNITY_EDITOR
            name = $"ServerUnit [ID {ownerPlayer.playerId}]: {ownerPlayer.nickname}";
            #endif
        }
        
        
    }
}
