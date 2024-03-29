using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDiceBagController : MonoBehaviour
{
    public List<SpawnableDice> spawnableDices;
    public List<DiceType> bagDices;
    public DiceConveyorBelt diceConveyorBelt;
    
    public float timeBetweenSpawns = 1f; 
    
    private int _currentDice = 0;
    public float currentSpawnTimer = 0f;

    public void AddDiceToBag(DiceType diceType)
    {
        var prevCurrent = bagDices[_currentDice];
        bagDices[_currentDice] = diceType;
        bagDices.Add(prevCurrent);
        
    }

    public Dice GetNextDice()
    {
        var spawnedDice = SpawnDiceByType(bagDices[_currentDice]);
        _currentDice++;
        if (_currentDice < bagDices.Count) return spawnedDice;
        _currentDice = 0;
        ShuffleDices();
        return spawnedDice;
    }

    private Dice SpawnDiceByType(DiceType diceType)
    {
        SpawnableDice diceToSpawn = spawnableDices.Find(dice => dice.diceType == diceType);
        var spawnedDice = Instantiate(diceToSpawn.dicePrefab, null);
        spawnedDice.gameObject.SetActive(true);
        return spawnedDice.GetComponent<Dice>();
    }
    
    private void ShuffleDices()
    {
        int n = bagDices.Count;  
        while (n > 1) { 
            n--;  
            int k = UnityEngine.Random.Range(0, n + 1);  
            (bagDices[k], bagDices[n]) = (bagDices[n], bagDices[k]);
        }
    }

    private void Start()
    {
        UnityEngine.Random.InitState((int)(DateTimeOffset.Now.ToUnixTimeSeconds() % int.MaxValue));
    }

    private void Update()
    {
        currentSpawnTimer += Time.deltaTime;
        if (!(currentSpawnTimer >= timeBetweenSpawns)) return;
        currentSpawnTimer -= timeBetweenSpawns;
        diceConveyorBelt.AddDiceToBelt(GetNextDice());
    }
}

[Serializable]
public enum DiceType
{
    SixSided,
    TwentySided,
    Multi,
    Trap,
}

[Serializable]
public struct SpawnableDice
{
    public Dice dicePrefab;
    public DiceType diceType;
}
