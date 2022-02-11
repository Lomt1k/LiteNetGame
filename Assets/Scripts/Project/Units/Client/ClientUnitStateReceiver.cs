using Project.Units.Server.Packets;

namespace Project.Units
{
    public class ClientUnitStateReceiver : UnitComponent
    {
        private ushort _lastReceivedPacketId;
        
        public void UpdateUnitState(UpdateUnitStatePacket packet)
        {
            if (IsNewestPacket(packet.packetId))
            {
                _lastReceivedPacketId = packet.packetId;
                ApplyUnitStateChanges(packet);
            }
        }
        
        private bool IsNewestPacket(ushort packetId)
        {
            return packetId > _lastReceivedPacketId || (packetId < 500 && _lastReceivedPacketId > 65000);
        }

        private void ApplyUnitStateChanges(UpdateUnitStatePacket packet)
        {
            //только для теста: позже добавлю интерполяцию
            unit.transform.position = packet.position;
            unit.transform.rotation = packet.rotation;
        }
        
        
    }
}
