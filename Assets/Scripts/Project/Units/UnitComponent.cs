using UnityEngine;

namespace Project.Units
{
    public class UnitComponent : MonoBehaviour
    {
        public UnitBase unit { get; private set; }

        public virtual void Initialize(UnitBase unit)
        {
            this.unit = unit;
        }
    }
}

