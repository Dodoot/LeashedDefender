using System;
using UnityEngine;

public enum EGhostType
{
    Basic,
    Cat,
    Magician
}

[Serializable]
public struct GhostSpawnInfo
{
    public EGhostType GhostType;
    public float Time;
}

[Serializable]
public struct GhostWave
{
    public GhostSpawnInfo[] Spawns;
}

[CreateAssetMenu(menuName = "GhostWaves")]
public class GhostWaves : ScriptableObject
{
    [SerializeField] private GhostWave[] _waves;

    public GhostWave[] Waves => _waves;
}
