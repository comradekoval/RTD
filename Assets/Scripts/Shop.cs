using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Shop : MonoBehaviour
{
    public List<DicePrice> dicePrices;
    public PlayerDiceBagController bagController;
    public TextMeshPro text;
    public int money = 100;

    private bool _didBuy = false;
    
    public bool DidBuy()
    {
        return _didBuy;
    }

    public void AddMoney(int getCurrentValue)
    {
        money += getCurrentValue;
    }

    public void BuyDice(string diceName)
    {
        _didBuy = true;
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

    private void Update()
    {
        text.text = $"{money}";
    }
}

[Serializable]
public struct DicePrice
{
    public DiceType diceType;
    public int price;
}