using Networking;
using UnityEngine;

namespace Project.Units.Server.Packets
{
    public class CreateContollableUnitPacket : ServerPacket
    {
        public Vector3 position { get; set; }
        public Quaternion rotation { get; set; }
    }
}
