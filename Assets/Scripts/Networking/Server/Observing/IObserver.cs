using System.Collections.Generic;
using LiteNetLib;

namespace Networking.Server.Observing
{
    public interface IObserver
    {
        NetPeer peer { get; }
        List<IObservableObject> observedObjects { get; }
        UnityEngine.Vector3 position { get; }

    }
}

