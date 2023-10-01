using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct GhostPrefabInfo
{
    public EGhostType Type;
    public Ghost Prefab;
}

public class EnemiesManager : MonoBehaviour
{
    [SerializeField] private float _spawnInterval = 1f;
    [SerializeField] private float _spawnRadius = 15f;
    [SerializeField] private GhostPrefabInfo[] _ghostPrefabs = null;
    [SerializeField] private GhostWaves _ghostWaves;

    private float _timer;
    private int _nextWaveIndex;
    private int _nextGhostIndex;
    private GhostWave _currentWave;

    private bool _isInWave;

    private readonly Dictionary<EGhostType, Ghost> _ghostPrefabsDict = new Dictionary<EGhostType, Ghost>();

    private void Awake()
    {
        _ghostPrefabsDict.Clear();
        foreach (var ghost in _ghostPrefabs)
        {
            _ghostPrefabsDict[ghost.Type] = ghost.Prefab;
        }
    }

    private void Update()
    {
        // TEMP
        if (Input.GetKeyDown(KeyCode.W))
        {
            StartWave();
        }
        // END TEMP

        UpdateWave();
    }

    private void UpdateWave()
    {
        if (_isInWave)
        {
            _timer += Time.deltaTime;

            if (_nextGhostIndex < _currentWave.Spawns.Length && _timer >= _currentWave.Spawns[_nextGhostIndex].Time)
            {
                var ghost = _ghostPrefabsDict[_currentWave.Spawns[_nextGhostIndex].GhostType];
                var spawnPosition = UnityEngine.Random.insideUnitCircle.normalized * _spawnRadius;

                Instantiate(ghost, spawnPosition, Quaternion.identity);

                _nextGhostIndex++;
            }
        }
    }

    private void StartWave()
    {
        _currentWave = _ghostWaves.Waves[_nextWaveIndex];
        _timer = 0;
        _nextGhostIndex = 0;
        _isInWave = true;

        _nextWaveIndex++;

        Debug.Log("Wave Started");
    }
}
