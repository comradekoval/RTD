using UnityEngine;

public class D6 : Dice
{
    public override Vector3 GetVectorForSide(int side)
    {
        return side switch
        {
            1 => new Vector3(0F, 0F, -1F),
            2 => new Vector3(-1F, 0F, 0F),
            3 => new Vector3(0F, 1F, 0F),
            4 => new Vector3(1F, 0F, 0F),
            5 => new Vector3(0F, -1F, 0F),
            6 => new Vector3(0F, 0F, 1F),
            _ => Vector3.zero
        };
    }

    public override int GetMaxValue()
    {
        return 6;
    }
}
