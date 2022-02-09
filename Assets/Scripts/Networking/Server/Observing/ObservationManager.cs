using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Project.Units.Server;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Networking.Server.Observing
{
    /*
     * Вычисления в этом классе будут занимать много процессорного времени при высоком онлайне (например, 1000 игроков)
     * Но более оптимальной реализации я пока не придумал.
     *
     * Реализовано асинхронное выполнение, чтобы для этих вычислений могло использоваться отдельное ядро процессора
     */
    public class ObservationManager
    {
        public const int observingFrequencyInMilliseconds = 1000;
        public const float observationDistance = 15f; //for test

        private List<ServerUnit> _units = new List<ServerUnit>();
        private List<IObserver> _observersWithoutUnits = new List<IObserver>();
        private List<IObservableObject> _objectsWithoutUnits = new List<IObservableObject>();

        private ServerUnit[] _unitsArray;
        private IObserver[] _observersWithoutUnitsArray;
        private IObservableObject[] _objectsWithoutUnitsArray;

        private readonly Stopwatch _stopwatch = new Stopwatch();
        
        public ObservationManager()
        {
            ObservationUpdate();
        }

        public void RegisterUnit(ServerUnit unit)
        {
            _units.Add(unit);
        }

        public void UnregisterUnit(ServerUnit unit)
        {
            _units.Remove(unit);
        }

        public void RegisterObserver(IObserver observer)
        {
            _observersWithoutUnits.Add(observer);
        }
        
        public void UnregisterObserver(IObserver observer)
        {
            _observersWithoutUnits.Remove(observer);
        }
        
        public void RegisterObservableObject(IObservableObject obj)
        {
            _objectsWithoutUnits.Add(obj);
        }
        
        public void UnregisterObservableObject(IObservableObject obj)
        {
            _objectsWithoutUnits.Remove(obj);
        }

        private async void ObservationUpdate()
        {
            while (Application.isPlaying)
            {
                _stopwatch.Restart();
                RebuildArrays();
                //Debug.Log($"Start ObservationUpdate | cycle time ms {_stopwatch.ElapsedMilliseconds}");
                
                await Task.Run(RefreshUnitObservers);
                //Debug.Log($"Unit Observers Refreshed | cycle time ms {_stopwatch.ElapsedMilliseconds}");

                await Task.Run(RefreshObserversWithoutUnit);
                //Debug.Log($"Another observers Refreshed | cycle time ms {_stopwatch.ElapsedMilliseconds}");

                var cycleWorkTime = _stopwatch.ElapsedMilliseconds;
                if (cycleWorkTime < observingFrequencyInMilliseconds)
                {
                    var delay = observingFrequencyInMilliseconds - (int)cycleWorkTime;
                    await Task.Delay(delay);
                }
            }
        }

        //выводим данные из листов в массивы, т.к. содержимое листа может измениться во время асинхронной работы с ним
        private void RebuildArrays()
        {
            _unitsArray = _units.ToArray();
            _observersWithoutUnitsArray = _observersWithoutUnits.ToArray();
            _objectsWithoutUnitsArray = _objectsWithoutUnits.ToArray();
        }

        private void RefreshUnitObservers()
        {
            for (int i = 0; i < _unitsArray.Length; i++)
            {
                var observer = _unitsArray[i];
                for (int j = i + 1; j < _unitsArray.Length; j++)
                {
                    RefreshObservingBetweenUnits(observer, _unitsArray[j]);
                }
                foreach (var obj in _objectsWithoutUnitsArray)
                {
                    RefreshObjectObservingStatus(observer, obj);
                }
            }
        }
        
        private void RefreshObserversWithoutUnit()
        {
            foreach (var observer in _observersWithoutUnitsArray)
            {
                foreach (var unit in _unitsArray)
                {
                    RefreshObjectObservingStatus(observer, unit);
                }
                foreach (var obj in _objectsWithoutUnitsArray)
                {
                    RefreshObjectObservingStatus(observer, obj);
                }
            }
        }

        private static void RefreshObservingBetweenUnits(ServerUnit unitA, ServerUnit unitB)
        {
            try
            {

                bool prevState = unitA.observedObjects.Contains(unitB);
                bool newState = Vector3.Distance(unitA.transformData.position, unitB.transformData.position) < observationDistance;

                if (newState == prevState)
                    return;

                if (newState)
                {
                    unitA.observers.Add(unitB);
                    unitA.observedObjects.Add(unitB);
                    unitA.OnAddObserver(unitB);

                    unitB.observers.Add(unitA);
                    unitB.observedObjects.Add(unitA);
                    unitB.OnAddObserver(unitA);
                }
                else
                {
                    unitA.observers.Remove(unitB);
                    unitA.observedObjects.Remove(unitB);
                    unitA.OnRemoveObserver(unitB);

                    unitB.observers.Remove(unitA);
                    unitB.observedObjects.Remove(unitA);
                    unitB.OnRemoveObserver(unitA);
                }

            }
            catch (NullReferenceException ex)
            {
                Debug.LogWarning($"catched NullRef: RefreshObservingBetweenUnits | " + ex.Message);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }

        private static void RefreshObjectObservingStatus(IObserver observer, IObservableObject obj)
        {
            try
            {
                
                bool prevState = observer.observedObjects.Contains(obj);
                bool newState = Vector3.Distance(observer.position, obj.position) < observationDistance;
            
                if (newState == prevState)
                    return;
            
                if (newState)
                {
                    observer.observedObjects.Add(obj);
                    obj.observers.Add(observer);
                    obj.OnAddObserver(observer);
                }
                else
                {
                    observer.observedObjects.Remove(obj);
                    obj.observers.Remove(observer);
                    obj.OnRemoveObserver(observer);
                }
                
            }
            catch (NullReferenceException ex)
            {
                Debug.LogWarning($"catched NullRef: RefreshObjectObservingStatus | " + ex.Message);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
            
        }
        
        
    }
}

