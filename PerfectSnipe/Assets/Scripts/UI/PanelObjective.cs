using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelObjective : MonoBehaviour
{
    public static PanelObjective Instance = null;

    public TextMeshProUGUI txtLevelNo;
    public Image imgVictim;
    public TextMeshProUGUI txtObjective;
    public Button btnPlay;

    [Header("Refererence")]
    public GameObject panelGame;
    public GameObject weaponCamera;
    private void Start()
    {
        if (Instance == null) Instance = this;
        btnPlay.onClick.AddListener(() => PlayCallBack());
        LoadPanelObjective();
    }

    public void LoadPanelObjective()
    {
        this.gameObject.SetActive(true);
        panelGame.SetActive(false);
        weaponCamera.SetActive(false);
        InitPanelObjective();
    }

    private void PlayCallBack()
    {
        GameManager.Instance.isGameStarted = true;
        panelGame.SetActive(true);
        PanelGame.Instance.IniatializePanel();
        weaponCamera.SetActive(true);

        this.gameObject.SetActive(false);
    }

    public void InitPanelObjective()
    {
        AppDelegate appDelegate = AppDelegate.SharedManager();
        int currLevel = appDelegate.tempLevelCounter + 1;
        txtLevelNo.text = "Level " + currLevel.ToString();
        Level level = LevelManager.Instance.GetLevelInfo(appDelegate.levelCounter);
        imgVictim.sprite = Resources.Load<Sprite>("Icon/" + "i" + level.victimId.ToString());
        txtObjective.text = "Find " + level.totalVictim + "  " + appDelegate.selectedDinoName + " shark and tranquilize them";
    }
}
