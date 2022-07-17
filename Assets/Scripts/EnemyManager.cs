using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{

    public Shop shop;
    public Player player;
    public PlayerDiceBagController dbc;
    public DiceConveyorBelt dcb;
    public GameState gameState;

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
        enemySpeedMultiplier = 1.7f,
        enemySpawnIntervalMax = 1.5f,
        enemySpawnIntervalMin = 1f
    };
    private readonly List<int> _completedWaves= new List<int>{};
    private int _currentWaveIndex = -1;
    private Goblin _currentBoss;
    private float _nextSpawnTime;
    
    
    public GameObject dropTip;
    public GameObject rollTip;
    public GameObject shopTip;
    private Vector3 shopTipPos;
    private Vector3 dropTipPos;
    private Vector3 rollTipPos;
    private bool showShopTip = true;
    private bool showDropTip = true;
    private bool showRollTip = true;
    public static bool didDropADie = false;
    public static bool didRollADie = false;
    
    private void Start()
    {
        didDropADie = false;
        didRollADie = false;
        shopTipPos = HideTip(shopTip);
        dropTipPos = HideTip(dropTip);
        rollTipPos = HideTip(rollTip);
    }

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
        MaybeHideHint();
        MaybeShowHint();
    }

    private void MaybeSpawnGoblin()
    {
        if(_nextSpawnTime >= Time.time) return;
        _nextSpawnTime = Time.time + (Random.Range(_currentWave.enemySpawnIntervalMin, _currentWave.enemySpawnIntervalMax));

        if (_willSpawnBoss)
        {
            _willSpawnBoss = false;
            _currentBoss = SpawnEnemyByType(_currentWave.boss,  new Vector3(0f, 0f, 0.3f));
            gameState.CallDementius();
            return;
        }
        
        _currentWavePowerLeft--;
        var enemyTypeToSpawn = _currentWave.enemyTypes[Random.Range(0, _currentWave.enemyTypes.Count)];
        var goblin = SpawnEnemyByType(enemyTypeToSpawn, new Vector3(0, 0, Random.Range(-2, 2.1f)));
        if (_currentWave.enemySpeedMultiplier != 0)
        {
            goblin.speed *= _currentWave.enemySpeedMultiplier;
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
                endlessWave.conveyorSpeed += 0.2f;
                endlessWave.diceSpawnInterval -= 0.1f;
                endlessWave.enemySpeedMultiplier += 0.2f;
                endlessWave.enemySpawnIntervalMax -= 0.1f;
                endlessWave.enemySpawnIntervalMin -= 0.1f;
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
        var newEnemy = Instantiate(enemyToSpawn.enemyPrefab, transform.position + offset, Quaternion.identity);
        return newEnemy;
    }

    private void MaybeShowHint()
    {
        if (_currentWave.name == "Try dice" && showShopTip)
        {
            showShopTip = false;
            shopTip.transform.position = shopTipPos;
        }
        if (_currentWave.name == "Tutorial" && showRollTip)
        {
            showRollTip = false;
            rollTip.transform.position = rollTipPos;
        }
        if (_currentWave.name == "Tutorial" && showDropTip)
        {
            showDropTip = false;
            dropTip.transform.position = dropTipPos;
        }
    }
    
    private void MaybeHideHint()
    {
        if (shop.DidBuy())
        {
            showShopTip = false;
            HideTip(shopTip);
        }

        if (didRollADie)
        {
            showRollTip = false;
            HideTip(rollTip);
        }
        
        if (didDropADie)
        {
            showDropTip = false;
            HideTip(dropTip);
        }
    }

    private static Vector3 HideTip(GameObject tip)
    {
        var pos = tip.transform.position;
        tip.transform.position = Vector3.forward * 200;
        return pos;
    }
}

[Serializable]
public enum EnemyType
{
    Goblin,
    [UsedImplicitly] Dementius,
    Pig,
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
    public float enemySpeedMultiplier;
    public float conveyorSpeed;
    public float diceSpawnInterval;
    public int power;
    public List<EnemyType> enemyTypes;
    public EnemyType boss;  
}
