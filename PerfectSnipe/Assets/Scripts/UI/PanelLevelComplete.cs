using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PanelLevelComplete : MonoBehaviour
{
    public TextMeshProUGUI txtLevel;
    public TextMeshProUGUI txtReward;
    public Button btnNoThanks;

    private AppDelegate appDelegate;

    Level level;
    private void Start()
    {
        appDelegate = AppDelegate.SharedManager();

        level = LevelManager.Instance.GetLevelInfo(appDelegate.levelCounter);

        txtLevel.text = (appDelegate.tempLevelCounter + 1).ToString();
        txtReward.text = level.rewardAmount.ToString();
        int resAmount = !PlayerPrefs.HasKey("Resource") ? 0 : PlayerPrefs.GetInt("Resource");
        PlayerPrefs.SetInt("Resource", resAmount + level.rewardAmount);
        btnNoThanks.onClick.AddListener(() => NoThanksCallBack());

        UiManager.Instance.PlayFx(true);
    }

    private void NoThanksCallBack()
    {
        UiManager.Instance.PlayFx(false);
        UiManager.Instance.LoadMainPanel();

        appDelegate.levelCounter = (appDelegate.levelCounter + 1 > 5) ?
            appDelegate.levelCounter = UnityEngine.Random.Range(0, 6) :
            appDelegate.levelCounter += 1;

        appDelegate.tempLevelCounter += 1;

        Destroy(this.gameObject);
    }
}
