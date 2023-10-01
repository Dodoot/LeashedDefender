using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Doggo _doggo = null;
    [SerializeField] private Human _human = null;

    public static Doggo Doggo => _instance._doggo;
    public static Human Human => _instance._human;
    public static Vector2 LeashDirection => (Human.transform.position - Doggo.transform.position).normalized;

    public static void Lose()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void Win()
    {
        // TODO
    }
}
