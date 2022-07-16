using UnityEngine;

public class D6 : Dice
{
    public override Vector3 GetVectorForSide(int side)
    {
        switch (side)
        {
            case 1: return new Vector3(0F, 0F, -1F);
            case 2: return new Vector3(-1F, 0F, 0F);
            case 3: return new Vector3(0F, 1F, 0F);
            case 4: return new Vector3(1F, 0F, 0F);
            case 5: return new Vector3(0F, -1F, 0F);
            case 6: return new Vector3(0F, 0F, 1F);
        }
        return Vector3.zero;
    }

    public override int GetMaxValue()
    {
        return 6;
    }
}
