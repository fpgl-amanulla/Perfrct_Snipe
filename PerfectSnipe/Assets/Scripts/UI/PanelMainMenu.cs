using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PanelMainMenu : MonoBehaviour
{
    public TextMeshProUGUI txtLevelNo;
    public Button btnPlay;
    LevelManager levelManager;

    void Start()
    {
        levelManager = LevelManager.Instance;
        txtLevelNo.text = "Level " + (AppDelegate.SharedManager().levelCounter + 1).ToString();
        btnPlay.onClick.AddListener(() => PlayCallBack());
    }

    private void PlayCallBack()
    {
        GameManager.Instance.weaponHolder.SetActive(true);
        GameManager.Instance.isLevelComplete = false;
        levelManager.LoadLevel(AppDelegate.SharedManager().levelCounter);
        UiManager.Instance.tapToAim.SetActive(true);
        UiManager.Instance.InitGamePanel();
        Destroy(this.gameObject);
    }

}
