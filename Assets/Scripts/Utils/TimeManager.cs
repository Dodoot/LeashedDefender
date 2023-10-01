using System.Collections;
using UnityEngine;

public class TimeManager : SingletonMultiScene<TimeManager>
{
    private float _timeSpeed = 1f;
    private bool _isPriorityEnabled;

    public static void SlowDown(float slowRatio, float duration, bool priority = false)
    {
        if (!_instance._isPriorityEnabled || priority)
        {
            _instance.StopAllCoroutines();
            _instance.StartCoroutine(_instance.SlowDownCoroutine(slowRatio, duration, priority));
        }
    }

    private IEnumerator SlowDownCoroutine(float slowRatio, float duration, bool priority)
    {
        _isPriorityEnabled = priority;
        Time.timeScale = slowRatio * _timeSpeed;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = _timeSpeed;
        _isPriorityEnabled = false;
    }

    public static void StopTime()
    {
        _instance.StopAllCoroutines();
        Time.timeScale = 0f;
    }

    public static void ResetTime()
    {
        Time.timeScale = _instance._timeSpeed;
    }
}
