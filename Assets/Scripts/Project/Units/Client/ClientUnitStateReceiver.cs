using Networking;
using Networking.Server;
using Project.Units.Client;
using Project.Units.Server.Packets;
using UnityEngine;

namespace Project.Units
{
    public class ClientUnitStateReceiver : UnitComponent
    {
        private ushort _lastReceivedPacketId;
        private float _lastReceivedPacketTime;
        private float _lerpProgress;
        private float _averageLerp;
        private float _lastPacketDelay;
        private Vector3 _startPos;
        private Vector3 _realPos;
        private Quaternion _startRot;
        private Quaternion _realRot;
        private Transform _transform;
        private ClientUnit _unit;
        private ServerConfig _config;
        private bool _isMine;

        public override void Initialize(UnitBase unit)
        {
            base.Initialize(unit);
            _transform = unit.transform;
            _unit = unit as ClientUnit;
            _isMine = _unit.isMine;
            _config = NetInfo.serverConfig;
            _averageLerp = _config.syncUnitState_rate;
            ResetInterpolation();
        }

        public void UpdateUnitState(UpdateUnitStatePacket packet)
        {
            
            if (IsNewestPacket(packet.packetId))
            {
                _lastReceivedPacketId = packet.packetId;
                //Debug.Log($"PacketId: {_lastReceivedPacketId} | delay: {Time.unscaledTime - _lastReceivedPacketTime}");
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
            _lastReceivedPacketTime = Time.unscaledTime;
            _lastPacketDelay = Time.unscaledTime - _lastPacketDelay;
            if (_lastPacketDelay > 0.25f)
            {
                _lastPacketDelay = _config.syncUnitState_rate;
            }

            _averageLerp = (_averageLerp + _lastPacketDelay) / 2;
            StartTransformInterpolation(packet.position, packet.rotation);
            DataTypes.NetDataTypes_Units.ApplyStateInfoToUnit(packet.stateInfo, _unit);
        }

        private void StartTransformInterpolation(Vector3 pos, Quaternion rot)
        {
            _lerpProgress = 0f;
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
            if (_lerpProgress < 1f)
            {
                _lerpProgress = (Time.unscaledTime - _lastReceivedPacketTime) / _averageLerp;
                _transform.position = Vector3.Lerp(_startPos, _realPos, _lerpProgress);
                _transform.rotation = !IsBadQuaternion(_realRot)
                    ? Quaternion.Lerp(_startRot, _realRot, _lerpProgress)
                    : _realRot;
            }
        }

        private static bool IsBadQuaternion(Quaternion q)
        {
            return q.x == 0.0 && q.y == 0 && q.z == 0 && q.w == 0;
        }
        
        
    }
}
