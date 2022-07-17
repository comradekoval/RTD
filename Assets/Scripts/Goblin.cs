using System;
using UnityEngine;

public class Goblin : MonoBehaviour
{
    public int hp = 1;
    public int maxHp = 6;
    private bool _isLeftFoot = true;
    private float _nextAnimationTime;

    public int dmg = 1;
    public float speed = 0.01f;
    public float animationCooldown = 0.3f;
    public ProgressBar progressBar;

    private void Start()
    {
        _nextAnimationTime = Time.time + animationCooldown;
        transform.Rotate(0, -5.0f, 0.0f, Space.World);
        hp = maxHp;
    }

    // Update is called once per frame
    private void FixedUpdate()
    { 
        transform.position += new Vector3(speed, 0, 0);
    }

    private void Update()
    {
        AnimateWalk();
        progressBar.progress = hp / (float)maxHp * 100f;
    }

    public void ReceiveDamage(int dmg)
    {
        hp -= dmg;
        if (hp > 0) return;
        
        hp = 0;
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
        if (trigger.gameObject.CompareTag("Trap"))
        {
            ReceiveDamage(2);
            Destroy(trigger.gameObject);
        }
        
        if (trigger.gameObject.CompareTag("Player"))
        {
            var playerScript = trigger.gameObject.GetComponent<Player>();
            playerScript.GetDamage(dmg);
            Die();
        }
        
    
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
