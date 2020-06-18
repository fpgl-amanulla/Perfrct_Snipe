using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Video;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance;

    public Camera uiCamera;
    public GameObject tapToAim;
    public GameObject levelCompletePrefab;
    public GameObject mainMenuPrefab;
    public GameObject popUpText;
    public TextMeshProUGUI txtLevelNo;

    [Header("Confetti FX")]
    public ParticleSystem confettifx01;
    public ParticleSystem confettifx02;

    void Start()
    {
        if (Instance == null) Instance = this;
        InitGamePanel();
    }

    void Update()
    {

    }
    public void ShowPopUptext()
    {
        popUpText.SetActive(true);
        StartCoroutine(PopUpTxtDisable());
    }
    IEnumerator PopUpTxtDisable()
    {
        yield return new WaitForSeconds(1.0f);
        popUpText.SetActive(false);
    }

    public void LoadLevelComplete()
    {
        Instantiate(levelCompletePrefab, this.transform);
        txtLevelNo.gameObject.SetActive(false);
    }

    internal void LoadMainPanel()
    {
        Instantiate(mainMenuPrefab, this.transform);
    }
    public void InitGamePanel()
    {
        txtLevelNo.gameObject.SetActive(true);
        txtLevelNo.text = "Level " + (AppDelegate.SharedManager().tempLevelCounter + 1).ToString();
    }

    public void PlayFx(bool status)
    {
        if (status)
        {
            this.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
            uiCamera.gameObject.SetActive(true);
            this.GetComponent<Canvas>().worldCamera = uiCamera;
            confettifx01.gameObject.SetActive(true);
            confettifx02.gameObject.SetActive(true);
            confettifx01.Play();
            confettifx02.Play();
        }
        else
        {
            this.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            uiCamera.gameObject.SetActive(false);
            confettifx01.Pause();
            confettifx02.Pause();
            confettifx01.gameObject.SetActive(false);
            confettifx02.gameObject.SetActive(false);
        }
    }
}
