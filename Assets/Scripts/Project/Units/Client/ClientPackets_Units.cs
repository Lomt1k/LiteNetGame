using Networking;
using Project.Units.DataTypes;
using UnityEngine;

namespace Project.Units.Client.Packets
{
    public class RequestCreateMineUnitPacket : ClientPacket { }

    public class UpdateMineUnitStatePacket : ClientPacket
    {
        public ushort packetId { get; set; } //unreliable: этот пакет может дублироваться или приходить не в том порядке
        public Vector3 position { get; set; }
        public Quaternion rotation { get; set; }
        public UnitStateInfo stateInfo { get; set; }
    }
    
    
}
