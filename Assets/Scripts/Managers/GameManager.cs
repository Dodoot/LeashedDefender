using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Doggo _doggo = null;
    [SerializeField] private Human _human = null;

    private static bool _isEnding;

    public static Doggo Doggo => _instance._doggo;
    public static Human Human => _instance._human;
    public static Vector2 LeashDirection => (Human.transform.position - Doggo.transform.position).normalized;

    private void Start()
    {
        _isEnding = false;
    }

    public static void Lose()
    {
        if (!_isEnding)
        {
            _isEnding = true;
            _instance.StartCoroutine(_instance.WaitAndLose());
        }
    }

    private IEnumerator WaitAndLose()
    {
        TimeManager.SlowDown(.3f, 1f);
        yield return new WaitForSecondsRealtime(1);
        SceneManager.LoadScene("Menu - Lose");
    }

    public static void Win()
    {
        if (!_isEnding)
        {
            _isEnding = true;
            _instance.StartCoroutine(_instance.WaitAndWin());
        }
    }

    private IEnumerator WaitAndWin()
    {
        TimeManager.SlowDown(.3f, 1f);
        yield return new WaitForSecondsRealtime(1);
        SceneManager.LoadScene("Menu - Win");
    }
}
