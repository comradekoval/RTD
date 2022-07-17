using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    public float progress = 100f;

    public Transform mask;
    public SpriteRenderer upperBar;
    public List<Color> colors;

    private void Update()
    {
        float progressOffset = (100f - progress) / 100f;
        mask.localPosition = new Vector3(-progressOffset, 0, 0);
        mask.localScale = new Vector3(mask.localScale.x, 1 - progressOffset, mask.localScale.z);
        
        if (colors.Count <= 0) return;
        Debug.Log(progress * colors.Count / 100f);
        Debug.Log((int)(progress * colors.Count / 100f));
        Debug.Log($"clamped {Mathf.Clamp((int) (progress * colors.Count / 100f), 0, colors.Count - 1)}");
        var newColor = colors[Mathf.Clamp((int) (progress * colors.Count / 100f), 0, colors.Count - 1)];
        upperBar.color = newColor;
    }
}
