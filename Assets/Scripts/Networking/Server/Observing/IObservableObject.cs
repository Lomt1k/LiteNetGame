using System.Collections.Generic;

namespace Networking.Server.Observing
{
    public interface IObservableObject
    {
        List<IObserver> observers { get; }
        UnityEngine.Vector3 position { get; }

        void OnAddObserver(IObserver observer);
        void OnRemoveObserver(IObserver observer);
    }
    
}
