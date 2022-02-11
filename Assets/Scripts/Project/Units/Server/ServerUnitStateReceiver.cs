using Project.Units.Client.Packets;
using UnityEngine;

namespace Project.Units
{
    public class ServerUnitStateReceiver : UnitComponent
    {
        private Transform _transform;
        private ushort _lastReceivedPacketId;

        public override void Initialize(UnitBase unit)
        {
            base.Initialize(unit);
            _transform = unit.transform;
        }

        public void OnReceiveNewData(UpdateMineUnitStatePacket packet)
        {
            if (!IsNewestPacket(packet.packetId))
                return;

            _lastReceivedPacketId = packet.packetId;
            
            //TODO Add some antiCheat check here
            ApplyUnitStateChanges(packet);
        }

        private bool IsNewestPacket(ushort packetId)
        {
            return packetId > _lastReceivedPacketId || (packetId < 500 && _lastReceivedPacketId > 65000);
        }

        private void ApplyUnitStateChanges(UpdateMineUnitStatePacket packet)
        {
            _transform.position = packet.position;
            _transform.rotation = packet.rotation;
        }
        
        
        
    }
}

