using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioController : MonoBehaviour
{
    public AudioSource audioSource;
    public List<AudioClip> explosionSoundClipList;

    public void PlayExplosionSound()
    {
        audioSource.PlayOneShot(explosionSoundClipList[Random.Range(0, explosionSoundClipList.Count)]);
    }
}
