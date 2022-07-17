using UnityEngine;

public class TrapDie : Dice
{
    public GameObject explodeInto;
    public float maxForce = 0.5f;
    public override void OnExplode()
    {
        var value = GetCurrentValue();
        for (var i = 0; i < value; ++i)
        {
            var newDice = Instantiate(explodeInto, transform.position, Quaternion.identity);
            
            var direction = new Vector3(Random.Range(-maxForce, maxForce), 0, Random.Range(-maxForce, maxForce));
            newDice.GetComponent<Trap>().SetTargetPosition(transform.position + direction); 
            //newDice.transform.position = transform.position + direction;
            //newDieRigidbody.AddForce(Vector3.up * 160f);
            //newDieRigidbody.AddForce(-direction * 100f);
            //newDieRigidbody.AddTorque(Random.insideUnitSphere.normalized * 4);
        }
        Destroy(gameObject);
    }

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