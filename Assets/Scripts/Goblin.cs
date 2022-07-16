using UnityEngine;

public class Goblin : MonoBehaviour
{
    private int _hp = 10;
    private bool _isLeftFoot = true;
    private float _nextAnimationTime;
    
    public float speed = 0.01f;
    public float animationCooldown = 0.3f;

    private void Start()
    {
        _nextAnimationTime = Time.time + animationCooldown;
        transform.Rotate(0, -5.0f, 0.0f, Space.World);
    }

    // Update is called once per frame
    private void FixedUpdate()
    { 
        transform.position += new Vector3(speed, 0, 0);
    }

    private void Update()
    {
        AnimateWalk();
    }

    public void ReceiveDamage(int dmg)
    {
        _hp -= dmg;
        if (_hp > 0) return;
        
        _hp = 0;
        Die();
    }
    
    private void AnimateWalk()
    {
        if(_nextAnimationTime >= Time.time) return;
       
        if (_isLeftFoot)
        {
            transform.Rotate(0, 10.0f, 0.0f, Space.World);
        }
        else
        {
            transform.Rotate(0, -10.0f, 0.0f, Space.World);
        }
        
        _nextAnimationTime = Time.time + animationCooldown;
        _isLeftFoot = !_isLeftFoot;
    }

    private void OnTriggerEnter(Collider trigger)
    {
        Debug.Log("OCE");
        Debug.Log(trigger.gameObject.tag);
        
        if (trigger.gameObject.CompareTag("Player"))
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
