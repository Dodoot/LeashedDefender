using UnityEngine;

public class GhostHurbox : MonoBehaviour
{
    [SerializeField] Ghost _ghost = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _ghost.Die();
    }
}
