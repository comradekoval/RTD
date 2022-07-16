using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class DiceConveyorBelt : MonoBehaviour
{
    public List<Transform> dicesOnBelt;
    public Transform beltStartPosition;

    public float conveyorSpeed = 1f;
    
    private Material _beltMaterial;

    public void AddDiceToBelt(Dice dice)
    {
        dicesOnBelt.Add(dice.transform);
        dice.AddToConveyorBelt(this);

        dice.transform.position = beltStartPosition.position;
    }
    
    public void RemoveDiceFromBelt(Transform diceTransform)
    {
        dicesOnBelt.Remove(diceTransform);
    }
    
    private void Start()
    {
        _beltMaterial = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        _beltMaterial.mainTextureOffset += new Vector2(conveyorSpeed * Time.deltaTime, 0);
        foreach (var diceOnBelt in dicesOnBelt)
        {
            diceOnBelt.position += transform.right * conveyorSpeed * Time.deltaTime;
        }
    }
}
