using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    public float progress = 100f;

    public Transform mask;
    
    private void Update()
    {
        float progressOffset = (100f - progress) / 100f;
        mask.localPosition = new Vector3(-progressOffset, 0, 0);
        mask.localScale = new Vector3(mask.localScale.x, 1 - progressOffset, mask.localScale.z);
    }
}
