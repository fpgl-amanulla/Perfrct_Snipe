using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance = null;

    public Camera uiCamera;
    public GameObject tapToAim;
    public GameObject levelCompletePrefab;
    public GameObject levelFailedPrefab;
    public GameObject mainMenuPrefab;
    public GameObject popUpText;
    public TextMeshProUGUI txtLevelNo;

    public void Awake()
    {
        if (Instance == null) Instance = this;
        InitGamePanel();
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

    public void LoadLevelFailed()
    {
        Instantiate(levelFailedPrefab, this.transform);
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
            //this.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
            //uiCamera.gameObject.SetActive(true);
            //this.GetComponent<Canvas>().worldCamera = uiCamera;
            FxManager.Instance.ConfettiSetActive(true);
            FxManager.Instance.PlayConfettiFx(true);
        }
        else
        {
            //this.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            //uiCamera.gameObject.SetActive(false);
            FxManager.Instance.ConfettiSetActive(false);
            FxManager.Instance.PlayConfettiFx(false);
        }
    }
}
