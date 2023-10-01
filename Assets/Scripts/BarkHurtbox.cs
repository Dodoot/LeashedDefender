using UnityEngine;

public class BarkHurtbox : MonoBehaviour
{
    [SerializeField] Ghost _ghost = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _ghost.Barked((transform.position - collision.transform.position).normalized);
    }
}
