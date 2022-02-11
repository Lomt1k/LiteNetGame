using Project.Units.Client.Packets;
using Project.Units.Server;
using UnityEngine;

namespace Project.Units
{
    public class ServerUnitStateReceiver : UnitComponent
    {
        private Transform _transform;
        private ushort _lastReceivedPacketId;
        private ServerUnit _unit;

        public override void Initialize(UnitBase unit)
        {
            base.Initialize(unit);
            _transform = unit.transform;
            _unit = unit as ServerUnit;
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

            SendStateChangesToObservers(packet);
        }

        private void SendStateChangesToObservers(UpdateMineUnitStatePacket packet)
        {
            foreach (var observer in _unit.observers)
            {
                if (observer == null)
                    continue;
                
                ServerSending_Units.SendUpdateUnitState(observer.peer, _unit, packet);
            }
        }
        
        
        
    }
}

