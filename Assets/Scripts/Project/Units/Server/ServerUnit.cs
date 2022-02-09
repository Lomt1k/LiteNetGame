using System.Collections.Generic;
using LiteNetLib;
using Networking.Server;
using Networking.Server.Observing;
using UnityEngine;

namespace Project.Units.Server
{
    public class ServerUnit : UnitBase, IObserver, IObservableObject
    {
        public ServerPlayer player { get; private set; }
        public NetPeer peer { get; private set; }
        public Vector3 position => transformN.position;
        public List<IObservableObject> observedObjects { get; } = new List<IObservableObject>();
        public List<IObserver> observers { get; } = new List<IObserver>();

        public Transform transformN { get; private set; }
        
        public void Initialize(ServerPlayer ownerPlayer)
        {
            player = ownerPlayer;
            peer = player.peer;
            DontDestroyOnLoad(gameObject);

            transformN = transform;
            
            #if UNITY_EDITOR
            name = $"ServerUnit [ID {ownerPlayer.playerId}]: {ownerPlayer.nickname}";
            #endif
        }


       
        public void OnAddObserver(IObserver observer)
        {
            throw new System.NotImplementedException();
        }

        public void OnRemoveObserver(IObserver observer)
        {
            throw new System.NotImplementedException();
        }
    }
}
