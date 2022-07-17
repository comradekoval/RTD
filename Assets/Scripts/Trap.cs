using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public Vector3 targetPosition;
    public float speed = 1f;

    private Vector3 _startPosition;
    private float _currentTime = 0f;
    private bool _isFlyingToTarget = false;

    public void SetTargetPosition(Vector3 target)
    {
        targetPosition = target;
        _startPosition = transform.position;
        _currentTime = 0f;
        _isFlyingToTarget = true;
    }

    private void Update()
    {
        if (!_isFlyingToTarget) return;
        if (_currentTime > 1f)
        {
            _currentTime = 1f;
            _isFlyingToTarget = false;
        }
        _currentTime += speed * Time.deltaTime;
        transform.position = Vector3.Lerp(_startPosition, targetPosition, _currentTime);
    }
}
