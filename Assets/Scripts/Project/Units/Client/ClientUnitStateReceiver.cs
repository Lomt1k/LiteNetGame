using Project.Units.Client;
using Project.Units.Server.Packets;
using UnityEngine;

namespace Project.Units
{
    public class ClientUnitStateReceiver : UnitComponent
    {
        private ushort _lastReceivedPacketId;
        private float _lastReceivedPacketTime;
        private float _lastPacketDelay;
        private float _currentSyncTime;
        private Vector3 _startPos;
        private Vector3 _realPos;
        private Quaternion _startRot;
        private Quaternion _realRot;
        private Transform _transform;
        private ClientUnit _unit;
        private bool _isMine;

        public override void Initialize(UnitBase unit)
        {
            base.Initialize(unit);
            _transform = unit.transform;
            _unit = unit as ClientUnit;
            _isMine = _unit.isMine;
            ResetInterpolation();
        }

        public void UpdateUnitState(UpdateUnitStatePacket packet)
        {
            if (IsNewestPacket(packet.packetId))
            {
                _lastReceivedPacketId = packet.packetId;
                ApplyUnitStateChanges(packet);
            }
        }

        public void ResetInterpolation()
        {
            _realPos = _startPos = _transform.position;
            _realRot = _startRot = _transform.rotation;
        }
        
        private bool IsNewestPacket(ushort packetId)
        {
            return packetId > _lastReceivedPacketId || (packetId < 500 && _lastReceivedPacketId > 65000);
        }

        private void ApplyUnitStateChanges(UpdateUnitStatePacket packet)
        {
            _lastPacketDelay = Mathf.Clamp(Time.unscaledTime - _lastReceivedPacketTime, 0f, 0.5f);
            _lastReceivedPacketTime = Time.unscaledTime;
            StartTransformInterpolation(packet.position, packet.rotation);
        }

        private void StartTransformInterpolation(Vector3 pos, Quaternion rot)
        {
            _currentSyncTime = 0f;
            _startPos = _transform.position;
            _startRot = _transform.rotation;
            _realPos = pos;
            _realRot = rot;
        }

        private void Update()
        {
            if (!_isMine)
            {
                TransformInterpolationProgress();
            }
        }

        private void TransformInterpolationProgress()
        {
            if (_currentSyncTime < _lastPacketDelay)
            {
                _currentSyncTime += Time.unscaledDeltaTime;
                var progress = _currentSyncTime / _lastPacketDelay;
                _transform.position = Vector3.Lerp(_startPos, _realPos, progress);
                _transform.rotation = !IsBadQuaternion(_realRot)
                    ? Quaternion.Lerp(_startRot, _realRot, progress)
                    : _realRot;
            }
        }

        private static bool IsBadQuaternion(Quaternion q)
        {
            return q.x == 0.0 && q.y == 0 && q.z == 0 && q.w == 0;
        }
        
        
    }
}
