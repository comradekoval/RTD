using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public TextMeshPro hpTMP;
    public TextMeshPro scoreTMP;
    public GameState gameState;
    public ScreenShake screenShake;

    public int hp = 10;
    public int maxHp = 10;

    private int _score;
    public int scorePerTick = 1;
    public float tickTime = 1f;
    private float _nextTickTime;
    
    // Start is called before the first frame update
    void Start()
    {
        hpTMP.text = $"{hp}/{maxHp}";
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        UpdateScore();
    }

    private void UpdateScore()
    {
        if(_nextTickTime >= Time.time) return;
        _nextTickTime = Time.time + tickTime;
        
        _score += scorePerTick;
        scoreTMP.text = $"{_score}";
    }

    public void GetDamage(int dmg)
    {
        hp -= dmg;
        hpTMP.text = $"{hp}/{maxHp}";
        screenShake.ShakeScreen();
        
        if (hp > 0) return;
        scorePerTick = 0;
        gameState.EndGame(_score);
    }
}
