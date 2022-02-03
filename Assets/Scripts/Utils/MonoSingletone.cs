using UnityEngine;

public class MonoSingletone<T> : MonoBehaviour where T: MonoBehaviour
{
    private static T _instance;

    public static T instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject(typeof(T).Name).AddComponent<T>();
            }
            return _instance;
        }
    }
}
