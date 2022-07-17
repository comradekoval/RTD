using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    public Player player;
    public PlayerDiceBagController dbc;
    public DiceConveyorBelt dcb;
    
    public List<SpawnableEnemy> spawnableEnemies;
    public List<Wave> waves;
    
    private Wave _currentWave;
    private int _currentWavePowerLeft;
    private bool _willSpawnBoss = false;
    public Wave endlessWave = new Wave()
    {
        name = "endless",
        boss = EnemyType.None,
        power = 20,
        conveyorSpeed = 2,
        restPeriod = 2,
        diceSpawnInterval = 1f,
        goblinSpeed = 0.017f,
        enemySpawnIntervalMax = 1.5f,
        enemySpawnIntervalMin = 1f
    };
    private readonly List<int> _completedWaves= new List<int>{};
    private int _currentWaveIndex = -1;
    private Goblin _currentBoss;
    private float _nextSpawnTime;

    private static readonly Quaternion SpriteOrientation = Quaternion.LookRotation(Vector3.down);

    private void Update()
    {
        // game over mode
        if (player.hp <= 0)
        {
            dcb.conveyorSpeed = 0;
            dbc.currentSpawnTimer = -100000f;
            return;
        }

        if (_willSpawnBoss)
        {
            MaybeSpawnGoblin();
            return;
        }
        
        if (_currentBoss && _currentBoss.hp > 0)
        {
            return;
        }

        MaybeSwitchWave();
        MaybeSpawnGoblin();
    }

    private void MaybeSpawnGoblin()
    {
        if(_nextSpawnTime >= Time.time) return;
        _nextSpawnTime = Time.time + (Random.Range(_currentWave.enemySpawnIntervalMin, _currentWave.enemySpawnIntervalMax));

        if (_willSpawnBoss)
        {
            _willSpawnBoss = false;
            _currentBoss = SpawnEnemyByType(_currentWave.boss, Vector3.zero);
            return;
        }
        
        _currentWavePowerLeft--;
        var goblin = SpawnEnemyByType(EnemyType.Goblin, new Vector3(0, 0, Random.Range(-2, 2.1f)));
        if (_currentWave.goblinSpeed != 0)
        {
            goblin.speed = _currentWave.goblinSpeed;
        }
        
        if (_currentWavePowerLeft <= 0  && _currentWave.boss != EnemyType.None)
        {
            _willSpawnBoss = true;
        }
    }

    private void MaybeSwitchWave()
    {
        if (_currentWavePowerLeft == 0)
        {
            _completedWaves.Add(_currentWaveIndex);
        }
        
        if (_completedWaves.Contains(_currentWaveIndex))
        {
            _currentWaveIndex++;
            if (_currentWave.name == "endless")
            {
                endlessWave.conveyorSpeed += 0.1f;
                endlessWave.diceSpawnInterval -= 0.05f;
                endlessWave.goblinSpeed += 0.01f;
                endlessWave.enemySpawnIntervalMax -= 0.05f;
                endlessWave.enemySpawnIntervalMin -= 0.05f;
            }
            
            _currentWave = _currentWaveIndex < waves.Count ? waves[_currentWaveIndex] : endlessWave;

            _currentWavePowerLeft = _currentWave.power;
            _nextSpawnTime = Time.time + _currentWave.restPeriod;

            dbc.timeBetweenSpawns = _currentWave.diceSpawnInterval;
            dcb.conveyorSpeed = _currentWave.conveyorSpeed;

            _willSpawnBoss = false;
        }
    }
    private Goblin SpawnEnemyByType(EnemyType enemyType, Vector3 offset)
    {
        var enemyToSpawn = spawnableEnemies.Find(enemy => enemy.enemyType == enemyType);
        var newEnemy = Instantiate(enemyToSpawn.enemyPrefab, transform.position + offset, SpriteOrientation);
        return newEnemy;
    }
}

[Serializable]
public enum EnemyType
{
    Goblin,
    [UsedImplicitly] Dementius,
    None
}

[Serializable]
public struct SpawnableEnemy
{
    public Goblin enemyPrefab;
    public EnemyType enemyType;
}

[Serializable]
public struct Wave
{
    public string name;
    public float restPeriod;
    public float enemySpawnIntervalMin;
    public float enemySpawnIntervalMax;
    public float goblinSpeed;
    public float conveyorSpeed;
    public float diceSpawnInterval;
    public int power;
    // public List<EnemyType> enemyTypes;
    public EnemyType boss;  
}
