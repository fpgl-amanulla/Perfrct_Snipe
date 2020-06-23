using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PanelMainMenu : MonoBehaviour
{
    public TextMeshProUGUI txtLevelNo;
    public TextMeshProUGUI txtResAmount;
    public Button btnPlay;
    LevelManager levelManager;


    void Start()
    {
        levelManager = LevelManager.Instance;
        UpdateResourceBar();
        txtLevelNo.text = "Level " + (AppDelegate.SharedManager().tempLevelCounter + 1).ToString();
        btnPlay.onClick.AddListener(() => PlayCallBack());
    }

    private void PlayCallBack()
    {
        GameManager.Instance.weaponHolder.SetActive(true);
        GameManager.Instance.isLevelComplete = false;
        Score.SharedManager().ResetScore();
        //levelManager.LoadLevel(AppDelegate.SharedManager().levelCounter);
        LevelManager.Instance.LoadLevel();
        UiManager.Instance.tapToAim.SetActive(true);
        UiManager.Instance.InitGamePanel();
        Destroy(this.gameObject);
    }

    public void UpdateResourceBar()
    {
        int resAmount = !PlayerPrefs.HasKey("Resource") ? 0 : PlayerPrefs.GetInt("Resource");
        txtResAmount.text = resAmount.ToString();
    }

}
