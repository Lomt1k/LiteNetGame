using System.Collections.Generic;
using LiteNetLib;
using Networking.Server;
using Networking.Server.Observing;
using UnityEngine;

namespace Project.Units.Server
{
    //используется в другом потоке, когда движок не дает доступ напрямую к Transform
    public struct TransformData
    {
        public Vector3 position;
        public Quaternion rotation;
    }
    
    public class ServerUnit : UnitBase, IObserver, IObservableObject
    {
        public ServerPlayer player { get; private set; }
        public NetPeer peer { get; private set; }
        public Vector3 position => transformData.position;
        public List<IObservableObject> observedObjects { get; } = new List<IObservableObject>();
        public List<IObserver> observers { get; } = new List<IObserver>();

        public Transform transformN { get; private set; }
        public TransformData transformData { get; private set; }
        public ServerUnitStateReceiver unitStateReceiver { get; private set; }
        
        public void Initialize(ServerPlayer ownerPlayer)
        {
            player = ownerPlayer;
            peer = player.peer;
            DontDestroyOnLoad(gameObject);

            transformN = transform;
            transformData = new TransformData
            {
                position = transformN.position,
                rotation = transformN.rotation
            };
            
            #if UNITY_EDITOR
            name = $"ServerUnit [ID {ownerPlayer.playerId}]: {ownerPlayer.nickname}";
            #endif
        }

        public override void AddUnitComponents()
        {
            base.AddUnitComponents();
            unitStateReceiver = AddUnitComponent<ServerUnitStateReceiver>();
        }

        public void OnAddObserver(IObserver observer)
        {
            ServerSending_Units.SendAddObservingUnit(observer.peer, this);
        }

        public void OnRemoveObserver(IObserver observer)
        {
            ServerSending_Units.SendRemoveObservingUnit(observer.peer, this);
        }

        private void Update()
        {
            //потом перенести из Update на момент обновления позиции юнита
            transformData = new TransformData
            {
                position = transformN.position,
                rotation = transformN.rotation
            };
            
        }
        
        
    }
}
