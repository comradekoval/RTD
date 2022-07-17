using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public Vector3 targetPosition;
    public float speed = 1f;

    private Vector3 startPosition;
    private float _currentTime = 0f;
    private bool isFlyingToTarget = false;

    public void SetTargetPosition(Vector3 target)
    {
        targetPosition = target;
        startPosition = transform.position;
        _currentTime = 0f;
        isFlyingToTarget = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!isFlyingToTarget) return;
        if (_currentTime > 1f)
        {
            _currentTime = 1f;
            isFlyingToTarget = false;
        }
        _currentTime += speed * Time.deltaTime;
        transform.position = Vector3.Lerp(startPosition, targetPosition, _currentTime);
    }
}