using UnityEngine;

namespace Project.Units.Client
{
    public class ClientMineUnitStateSender : UnitComponent
    {
        public const float sendRate = 0.05f;
        public const float minFloatChangeSync = 0.001f;

        private Transform _transform;
        private Vector3 _lastSendedPos;
        private Quaternion _lastSendedRot;

        public override void Initialize(UnitBase unit)
        {
            base.Initialize(unit);
            _transform = unit.transform;
            _lastSendedPos = _transform.position;
            _lastSendedRot = _transform.rotation;
            InvokeRepeating(nameof(SendUpdate), sendRate, sendRate);
        }

        private void SendUpdate()
        {
            if (HasAnyStateUpdates())
            {
                SendDataToServer();
            }
        }

        private bool HasAnyStateUpdates()
        {
            return Vector3.Distance(_lastSendedPos, _transform.position) > minFloatChangeSync
                   || _lastSendedRot != _transform.rotation;
        }

        private void SendDataToServer()
        {
            _lastSendedPos = _transform.position;
            _lastSendedRot = _transform.rotation;
            ClientSending_Units.SendMineUnitStateUpdate(_lastSendedPos, _lastSendedRot);
        }
        
        
    }
}
