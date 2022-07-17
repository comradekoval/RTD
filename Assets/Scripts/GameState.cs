using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class GameState : MonoBehaviour
{
    public GameObject endGameUI;
    public Text endGameText;
    
    public void EndGame(int score)
    {
        var ppVol = FindObjectOfType<PostProcessVolume>();
        ppVol.profile.GetSetting<Vignette>().intensity.value = 1f;
        endGameUI.SetActive(true);
        endGameText.text = $"Final Score: {score}\nPress F5 to restart";
    }
}
