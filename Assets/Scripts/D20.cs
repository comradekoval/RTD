using UnityEngine;

public class D20 : Dice
{
    public override Vector3 GetVectorForSide(int side)
    {
        return side switch
        {
            1 => new Vector3(-0.2f, -0.6f, -0.8f),
            2 => new Vector3(-0.6f, 0f, -0.8f),
            3 => new Vector3(-0.2f, 0.6f, -0.8f),
            4 => new Vector3(0.5f, 0.4f, -0.8f),
            5 => new Vector3(0.5f, -0.4f, -0.8f),
            
            6 => new Vector3(0.2f, -0.6f, 0.8f),
            7 => new Vector3(0.6f, 0.0f, 0.8f),
            8 => new Vector3(0.2F, 0.6f, 0.8f),
            9 => new Vector3(-0.5f, 0.4f, 0.8f),
            10 => new Vector3(-0.5f, -0.4f, 0.8f),
            
            11 => new Vector3(-0.3f, -0.9f, -0.2f),
            12 => new Vector3(0.3f, -0.9f, 0.2f),
            13 => new Vector3(0.8f, -0.6f, -0.2f),
            14 => new Vector3(1f, 0f, 0.2f),
            15 => new Vector3(0.8f, 0.6f, -0.2f),
            
            16 => new Vector3(0.3f, 0.9f, 0.2f),
            17 => new Vector3(-0.3f, 0.9f, -0.2f),
            18 => new Vector3(-0.8f, 0.6f, 0.2f),
            19 => new Vector3(-1f, 0f, -0.2f),
            20 => new Vector3(-0.8f, -0.6f, 0.2f),
            _ => Vector3.zero
        };
    }
    
    public override int GetMaxValue()
    {
        return 20;
    }
}