using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Reflection;
using UnityEngine;

namespace Project.UI.Windows
{
    public class WindowsManager : MonoBehaviour
    {
        private static WindowsManager instance;

        [SerializeField, ID("Layer")]
        private LayersDictionary _layers;

        public static Action<Window> onWindowClosed;

        private void Awake()
        {
            instance = this;
        }

        public static T CreateWindow<T>(Layer layer = Layer.DefaultLayer) where T: Window
        {
            BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            FieldInfo fieldInfo = (typeof(T)).GetField("prefabPath", bindFlags);
            string prefabPath = (string)fieldInfo.GetValue(null);

            Transform parent = instance._layers[layer];
            T createdWindow = Instantiate(Resources.Load<GameObject>(prefabPath), parent).GetComponent<T>();
            createdWindow.onClose += () => onWindowClosed?.Invoke(createdWindow);

            return createdWindow;
        }
    }


    public enum Layer : byte
    {
        InGameHUD = 0,
        DefaultLayer,
        ImportantLayer,
        TechnicalLayer
    }

    [Serializable] public class LayersDictionary : SerializableDictionaryBase<Layer, Transform> { }
}
