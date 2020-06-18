using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource audioSource;
    public AudioClip shoot;
    public AudioClip explotion;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (Instance == null) Instance = this;
    }

    public void PlayShootSound() => audioSource.PlayOneShot(shoot);

    public void PlayExplosiveSound() => audioSource.PlayOneShot(explotion);
}
