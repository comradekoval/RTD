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