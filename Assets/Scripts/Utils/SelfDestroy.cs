using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    public void DestroySelf() // called by animator
    {
        Destroy(gameObject);
    }
}
