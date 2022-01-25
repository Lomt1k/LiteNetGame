using System.Collections.Generic;
using UnityEngine;

namespace Networking
{
    public abstract class ServerPacket { }
    
    public class AfterJoinInfoPacket : ServerPacket
    {
        public ushort maxPlayers;
        public ushort minePlayerId;
        //public SendablePlayerData testData;
        public Vector3 vector3;
        //public SendablePlayerData[] playersData;
    }
}