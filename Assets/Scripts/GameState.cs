using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameState : MonoBehaviour
{
    public GameObject endGameUI;
    public Text endGameText;
    public AudioSource source;
    public AudioClip dementiusIntro;

    private bool _isGameEnd = false;
    
    public void EndGame(int score)
    {
        var ppVol = FindObjectOfType<PostProcessVolume>();
        ppVol.profile.GetSetting<Vignette>().intensity.value = 1f;
        endGameUI.SetActive(true);
        endGameText.text = $"Final Score: {score}";
        _isGameEnd = true;
    }

    public void CallDementius()
    {
        source.PlayOneShot(dementiusIntro, 1f);
    }

    public void Restart()
    {
        if(!_isGameEnd) return;
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }
}
