using Networking;
using Project.Units.DataTypes;
using UnityEngine;

namespace Project.Units.Server.Packets
{
    public class CreateContollableUnitPacket : ServerPacket
    {
        public Vector3 position { get; set; }
        public Quaternion rotation { get; set; }
    }

    public class AddObservingUnitPacket : ServerPacket
    {
        public ushort playerId { get; set; }
        public Vector3 position { get; set; }
        public Quaternion rotation { get; set; }
        public UnitStateInfo stateInfo { get; set; }
    }

    public class RemoveObservingUnitPacket : ServerPacket
    {
        public ushort playerId { get; set; }
    }
    
    public class UpdateUnitStatePacket : ServerPacket
    {
        public ushort playerId { get; set; }
        public ushort packetId { get; set; }
        public Vector3 position { get; set; }
        public Quaternion rotation { get; set; }
        public UnitStateInfo stateInfo { get; set; }
    }
    
    
}
