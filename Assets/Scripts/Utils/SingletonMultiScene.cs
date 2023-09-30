using UnityEngine;

public abstract class SingletonMultiScene<T> : MonoBehaviour where T : Component
{
    protected static T _instance;

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this as T)
        {
            Destroy(gameObject);
        }
        else 
        {
            DontDestroyOnLoad(gameObject);
        }
    }

}