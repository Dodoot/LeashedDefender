using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    [SerializeField] private float _spawnInterval = 1f;
    [SerializeField] private float _spawnRadius = 15f;
    [SerializeField] private Ghost[] _ghostPrefabs = null;

    private float _timer;

    private void Update()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0)
        {
            var ghost = _ghostPrefabs[Random.Range(0, _ghostPrefabs.Length)];
            var spawnPosition = Random.insideUnitCircle.normalized * _spawnRadius;
            
            Instantiate(ghost, spawnPosition, Quaternion.identity);

            _timer = _spawnInterval;
        }
    }
}
