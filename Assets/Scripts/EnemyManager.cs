using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    private float _nextSpawnTime;
    public float spawnCooldown = 3f;

    public GameObject enemy;

    private void Start()
    {
        _nextSpawnTime = Time.time + 5f;
    }

    private void FixedUpdate()
    {
        MaybeSpawnGoblin();
    }

    private void MaybeSpawnGoblin()
    {
        if(_nextSpawnTime >= Time.time) return;
        _nextSpawnTime = Time.time + (spawnCooldown * Random.Range(0.5f, 1.2f));
        
        var newGoblin = Instantiate(enemy, transform.position + new Vector3(0, 0, Random.Range(-2, 2.1f)), Quaternion.identity);
        newGoblin.transform.Rotate(90f, 0, 0);
    }
}
