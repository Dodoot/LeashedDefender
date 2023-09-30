using UnityEngine;

public class Human : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float leashTensionThreshold = .5f;
    [SerializeField] private float leashMoveForceSpeedMultiplier = .2f;


    [Header("Inner References")]
    [SerializeField] private Rigidbody2D _rigidBody = null;

    public void Move(float leashTension)
    {
        if (leashTension >= 0)
        {
            _rigidBody.velocity = leashMoveForceSpeedMultiplier * leashTension * -GameManager.LeashDirection;
        }
        else
        {
            _rigidBody.velocity = Vector2.zero;
        }
    }
}
