using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Doggo _doggo = null;
    [SerializeField] private Human _human = null;

    public static Doggo Doggo => _instance._doggo;
    public static Human Human => _instance._human;
}
