using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leash : MonoBehaviour
{
    [SerializeField] private Transform _start = null;
    [SerializeField] private Transform _end = null;

    [SerializeField] private Transform[] _leashPoints = null;

    [SerializeField] private LineRenderer _lineRenderer = null;

    private void Update()
    {
        _start.position = GameManager.Human.LeashPoint.position;
        _end.position = GameManager.Doggo.LeashPoint.position;

        _lineRenderer.positionCount = _leashPoints.Length;

        for (int i = 0; i < _leashPoints.Length; i++)
        {
            _lineRenderer.SetPosition(i, _leashPoints[i].position);
        }
    }
}
