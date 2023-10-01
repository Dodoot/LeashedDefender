using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;

[Serializable]
public struct GhostPrefabInfo
{
    public EGhostType Type;
    public Ghost Prefab;
}

public class EnemiesManager : Singleton<EnemiesManager>
{
    [SerializeField] private float _spawnInterval = 1f;
    [SerializeField] private float _spawnRadius = 15f;
    [SerializeField] private GhostPrefabInfo[] _ghostPrefabs = null;
    [SerializeField] private GhostWaves _ghostWaves;
    [SerializeField] private Transform _ghostsHolder;

    [Header("UI")]
    [SerializeField] private Animator _animator;
    [SerializeField] private TMP_Text _waveText;

    private float _timer;
    private int _nextWaveIndex;
    private int _nextGhostIndex;
    private GhostWave _currentWave;

    private bool _isInWave;
    private bool _isWaitingForEnd;

    private readonly Dictionary<EGhostType, Ghost> _ghostPrefabsDict = new Dictionary<EGhostType, Ghost>();

    protected override void Awake()
    {
        base.Awake();

        _ghostPrefabsDict.Clear();
        foreach (var ghost in _ghostPrefabs)
        {
            _ghostPrefabsDict[ghost.Type] = ghost.Prefab;
        }
    }

    private void Update()
    {
        UpdateWave();
        CheckWaveEnd();
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

                var instance = Instantiate(ghost, spawnPosition, Quaternion.identity);
                instance.transform.SetParent(_ghostsHolder);

                _nextGhostIndex++;

                if (_nextGhostIndex < _currentWave.Spawns.Length)
                {
                    _isWaitingForEnd = true;
                }
            }
        }
    }

    private void CheckWaveEnd()
    {
        if (_isWaitingForEnd && _ghostsHolder.childCount == 0)
        {
            EndWave();
            _isWaitingForEnd = false;
        }
    }

    public static void StartWavePlease()
    {
        _instance.StartWave();
    }

    private void StartWave()
    {
        StartCoroutine(StartWaveCoroutine());
    }

    private void EndWave()
    {
        if (_nextWaveIndex < _ghostWaves.Waves.Length)
        {
            StartWave();
        }
        else
        {
            GameManager.Win();
        }
    }

    private IEnumerator StartWaveCoroutine()
    {
        _animator.SetTrigger("WaveStart");
        _waveText.text = $"Wave {_nextWaveIndex + 1}";

        yield return new WaitForSeconds(1f);

        _currentWave = _ghostWaves.Waves[_nextWaveIndex];
        _timer = 0;
        _nextGhostIndex = 0;
        _isInWave = true;

        _nextWaveIndex++;
    }
}
