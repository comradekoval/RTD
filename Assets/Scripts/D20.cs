using UnityEngine;

public class D20 : Dice
{
    public override Vector3 GetVectorForSide(int side)
    {
        switch (side)
        {
            case 1: return new Vector3(-0.2f, -0.6f,  -0.8f);
            case 2: return new Vector3(-0.6f, 0f,     -0.8f);
            case 3: return new Vector3(-0.2f, 0.6f,   -0.8f);
            case 4: return new Vector3(0.5f,  0.4f,   -0.8f);
            case 5: return new Vector3(0.5f,  -0.4f,  -0.8f);
            
            case 6: return new Vector3( 0.2f,  -0.6f, 0.8f); 
            case 7: return new Vector3( 0.6f,  0.0f,  0.8f);
            case 8: return new Vector3( 0.2F,  0.6f,  0.8f);
            case 9: return new Vector3( -0.5f, 0.4f,  0.8f);
            case 10: return new Vector3(-0.5f, -0.4f, 0.8f);
            
            case 11: return new Vector3(-0.3f, -0.9f, -0.2f);
            case 12: return new Vector3(0.3f,  -0.9f, 0.2f);
            case 13: return new Vector3(0.8f,  -0.6f, -0.2f);
            case 14: return new Vector3(1f,    0f,    0.2f);
            case 15: return new Vector3(0.8f,  0.6f,  -0.2f);
            
            case 16: return new Vector3(0.3f,  0.9f,  0.2f);
            case 17: return new Vector3(-0.3f, 0.9f,  -0.2f);
            case 18: return new Vector3(-0.8f, 0.6f,  0.2f);
            case 19: return new Vector3(-1f,   0f,    -0.2f);
            case 20: return new Vector3(-0.8f, -0.6f, 0.2f);
        }
        return Vector3.zero;
    }
    
    public override int GetMaxValue()
    {
        return 20;
    }
}