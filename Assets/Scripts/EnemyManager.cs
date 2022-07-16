using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    private float _nextSpawnTime;
    public float spawnCooldown = 1f;

    public GameObject enemy;

    // Update is called once per frame
    private void FixedUpdate()
    {
        MaybeSpawnGoblin();
    }

    private void MaybeSpawnGoblin()
    {
        if(_nextSpawnTime >= Time.time) return;
        _nextSpawnTime = Time.time + spawnCooldown;
        
        var newGoblin = Instantiate(enemy, transform.position + new Vector3(0, 0, Random.Range(-2, 2.1f)), Quaternion.identity);
        newGoblin.transform.Rotate(90f, 0, 0);
    }
}
