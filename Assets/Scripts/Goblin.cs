using System;
using UnityEngine;

public class Goblin : MonoBehaviour
{
    public SpriteRenderer normal;
    public SpriteRenderer trapped;
    public bool canBeTrapped = true;
    public int hp = 1;
    public int maxHp = 6;
    private bool _isLeftFoot = true;
    private float _nextAnimationTime;

    public int dmg = 1;
    public float speed = 0.01f;
    public float animationCooldown = 0.3f;
    public ProgressBar progressBar;

    public float inTrapDelay = 3f;
    
    private float _trapEscapeTime = 0;
    private bool _isTrapped = false;

    private void Start()
    {
        _nextAnimationTime = Time.time + animationCooldown;
        transform.Rotate(0, -5.0f, 0.0f, Space.World);
        hp = maxHp;
    }

    private void FixedUpdate()
    {
        if (_isTrapped) return;
        transform.position += new Vector3(speed, 0, 0);
    }

    private void Update()
    {
        MaybeEscapeTrap();
        AnimateWalk();
        progressBar.progress = hp / (float)maxHp * 100f;
    }

    public void ReceiveDamage(int dmgToReceive)
    {
        hp -= dmgToReceive;
        if (hp > 0) return;
        
        hp = 0;
        Die();
    }
    
    private void AnimateWalk()
    {
        if(_isTrapped) return;
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
        
        
        if (trigger.gameObject.CompareTag("Player"))
        {
            var playerScript = trigger.gameObject.GetComponent<Player>();
            playerScript.GetDamage(dmg);
            Die();
        }
    }

    private void OnTriggerStay(Collider trigger)
    {
        if (trigger.gameObject.CompareTag("Trap"))
        {
            GetTrapped(trigger.gameObject);
        }
    }

    private void MaybeEscapeTrap()
    {
        if(_trapEscapeTime >= Time.time) return;

        _isTrapped = false;
        normal.enabled = true;
        trapped.enabled = false;
        _trapEscapeTime = 0;
    }

    private void GetTrapped(GameObject trap)
    {
        
        if(_isTrapped) return;

        if (canBeTrapped)
        {
            _isTrapped = true;
            normal.enabled = false;
            trapped.enabled = true;
            _trapEscapeTime = Time.time + inTrapDelay;
        }

        Destroy(trap.gameObject);
        ReceiveDamage(2);
    }
    
    private void Die()
    {
        Destroy(gameObject);
    }
}
