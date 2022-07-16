using UnityEngine;

public class DamageZone : MonoBehaviour
{ 
    public int dmg = 6;
    public float lifeTime = 0.5f;
    private float _dieTime;
    
    private void Start()
    {
        _dieTime = Time.time + lifeTime;
    }

    private void FixedUpdate()
    {
        if (Time.time >= _dieTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider trigger)
    {
        if (!trigger.gameObject.CompareTag("Enemy")) return;
        
        var enemyScript = trigger.gameObject.GetComponent<Goblin>();
        enemyScript.ReceiveDamage(dmg);
    }
}
