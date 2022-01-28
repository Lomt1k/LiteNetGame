using UnityEngine;

namespace Project.UI.Windows
{
    public class Window : MonoBehaviour
    {
        public System.Action onClose;

        public virtual void OnCreated()
        {
        }

        public virtual void Close()
        {
            onClose?.Invoke();
            Destroy(gameObject);
        }
    }
}
