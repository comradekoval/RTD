using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceShopCollector : MonoBehaviour
{
    public Shop shop;
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.layer);
        Debug.Log(LayerMask.NameToLayer("Dice"));
        if (other.gameObject.layer != LayerMask.NameToLayer("Dice")) return;
        other.GetComponent<Dice>().CollectDice(shop);
    }
}
