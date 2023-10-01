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

        if (GameManager.Doggo.IsLeashTense)
        {
            _lineRenderer.positionCount = 2;

            _lineRenderer.SetPosition(0, _leashPoints[0].position);
            _lineRenderer.SetPosition(1, _leashPoints[_leashPoints.Length - 1].position);
        }
        else
        {
            _lineRenderer.positionCount = _leashPoints.Length;

            for (int i = 0; i < _leashPoints.Length; i++)
            {
                _lineRenderer.SetPosition(i, _leashPoints[i].position);
            }
        }

        var color = GameManager.Doggo.GetChargeLevel().LeashColor;
        _lineRenderer.startColor = color;
        _lineRenderer.endColor = color;
    }
}
