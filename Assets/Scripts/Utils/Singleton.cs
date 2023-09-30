using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Component
{
    protected static T _instance;

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
        }
        else if (_instance != this as T)
        {
            Destroy(gameObject);
        }
    }

}