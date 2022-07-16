using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public List<DicePrice> dicePrices;
    public PlayerDiceBagController bagController;
    public TextMeshPro text;
    public int money = 100;

    public void AddMoney(int getCurrentValue)
    {
        money += getCurrentValue;
    }

    public void BuyDice(string diceName)
    {
        if (Enum.TryParse(diceName, out DiceType diceType))
        {
            int dicePriceForType = dicePrices.Find(dicePrice => dicePrice.diceType == diceType).price;
            if (money < dicePriceForType) return;
            money -= dicePriceForType;
            bagController.AddDiceToBag(diceType);
        }
        else
        {
            Debug.LogError("wrong diceName");
        }
    }

    public void BuyDiceA(DiceType type)
    {
        
    }

    private void Update()
    {
        text.text = $"money: {money}";
    }
}

[Serializable]
public struct DicePrice
{
    public DiceType diceType;
    public int price;
}