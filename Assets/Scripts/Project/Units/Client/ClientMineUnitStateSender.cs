using Project.Units.DataTypes;
using UnityEngine;

namespace Project.Units.Client
{
    public class ClientMineUnitStateSender : UnitComponent
    {
        public const float sendRate = 0.04f;
        public const float minFloatChangeSync = 0.01f;

        private ClientUnit _clientUnit;
        private Transform _transform;
        private Vector3 _lastSendedPos;
        private Quaternion _lastSendedRot;
        private UnitStateInfo _lastSendedStateInfo;

        public override void Initialize(UnitBase unit)
        {
            base.Initialize(unit);
            _clientUnit = unit as ClientUnit;
            _transform = unit.transform;
            _lastSendedPos = _transform.position;
            _lastSendedRot = _transform.rotation;
            InvokeRepeating(nameof(SendUpdate), sendRate, sendRate);
        }

        private void SendUpdate()
        {
            var stateInfo = NetDataTypes_Units.GetStateInfoFromUnit(_clientUnit);
            if (HasAnyStateUpdates(stateInfo))
            {
                SendDataToServer(stateInfo);
            }
        }

        private bool HasAnyStateUpdates(UnitStateInfo stateInfo)
        {
            return Vector3.Distance(_lastSendedPos, _transform.position) > minFloatChangeSync
                   || _lastSendedRot != _transform.rotation
                   || Mathf.Abs(stateInfo.speed - _lastSendedStateInfo.speed) > minFloatChangeSync
                   || stateInfo.bitArray_0 != _lastSendedStateInfo.bitArray_0;
        }

        private void SendDataToServer(UnitStateInfo stateInfo)
        {
            _lastSendedPos = _transform.position;
            _lastSendedRot = _transform.rotation;
            _lastSendedStateInfo = stateInfo;
            
            ClientSending_Units.SendMineUnitStateUpdate(_lastSendedPos, _lastSendedRot, _lastSendedStateInfo);
        }
        
        
    }
}
