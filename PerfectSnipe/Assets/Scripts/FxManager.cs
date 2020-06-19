using System;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;

public class FxManager : MonoBehaviour
{
    public static FxManager Instance;

    [Header("FX")]
    public GameObject bulletImpactFX;
    public GameObject explosiveImpactFX;

    [Header("Confetti FX")]
    public ParticleSystem confettifx01;
    public ParticleSystem confettifx02;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void PlayConfettiFx(bool status)
    {
        if (status)
        {
            confettifx01.Play();
            confettifx02.Play();
        }
        else
        {
            //confettifx01.Pause();
            //confettifx02.Pause();
        }
    }
    public void ConfettiSetActive(bool task)
    {
        confettifx01.gameObject.SetActive(task);
        confettifx02.gameObject.SetActive(task);
    }

}
