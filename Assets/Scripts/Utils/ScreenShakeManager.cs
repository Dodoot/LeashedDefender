using UnityEngine;

public class ScreenShakeManager : MonoBehaviour
{
    [SerializeField] Camera mainCamera = null;

    private float globalShakeAmount = 0f;

    private float tempShakeAmount = 0f;
    private float tempShakeTimer = 0f;

    private static ScreenShakeManager instance;

    public static void SetGlobalShake(float value)
    {
        instance.globalShakeAmount = value;
    }

    public static void SetTempShake(float value, float time)
    {
        instance.tempShakeAmount = value;
        instance.tempShakeTimer = time;
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Update()
    {
        var shakeAmount = globalShakeAmount;

        if (tempShakeTimer > 0)
        {
            tempShakeTimer -= Time.deltaTime;

            shakeAmount += tempShakeAmount * tempShakeTimer;
        }
        else
        {
            tempShakeTimer = 0f;
        }

        mainCamera.transform.localPosition = new Vector3(Random.Range(-1f, 1f) * shakeAmount, Random.Range(-1f, 1f) * shakeAmount, mainCamera.transform.localPosition.z);
    }
}