using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ScreenShake : MonoBehaviour
{
    public float shakeTime = 0.2f;
    public float shakeMangitude = 0.1f;

    private Vector3 _defaultPosition;
    private float _currentShakeTime;
    private bool _isShaking;
    
    public void ShakeScreen()
    {
        _isShaking = true;
        _currentShakeTime = 0f;
    }

    private void Awake()
    {
        _defaultPosition = transform.position;
    }

    private void LateUpdate()
    {
        if (!_isShaking) return;
        float xMagnitude = Random.Range(-shakeMangitude, shakeMangitude);
        float yMagnitude = Random.Range(-shakeMangitude, shakeMangitude);

        transform.position += new Vector3(xMagnitude, yMagnitude, 0f);
        _currentShakeTime += Time.deltaTime;
        if (_currentShakeTime >= shakeTime)
        {
            transform.position = _defaultPosition;
        }
    }
}
