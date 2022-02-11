using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Units
{
    public abstract class UnitBase : MonoBehaviour
    {
        private readonly Dictionary<Type, UnitComponent> _unitComponents = new Dictionary<Type, UnitComponent>();

        private void Start()
        {
            AddUnitComponents();
        }

        public virtual void AddUnitComponents()
        {
            
        }

        protected T AddUnitComponent<T>() where T : UnitComponent
        {
            var component = gameObject.AddComponent<T>();
            component.Initialize(this);
            _unitComponents.Add(typeof(T), component);
            return component;
        }

        public T GetUnitComponent<T>() where T : UnitComponent
        {
            return _unitComponents[typeof(T)] as T;
        }
        
    }
}

