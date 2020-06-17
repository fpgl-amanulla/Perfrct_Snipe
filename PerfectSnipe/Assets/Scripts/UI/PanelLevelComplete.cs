using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PanelLevelComplete : MonoBehaviour
{
    public TextMeshProUGUI txtLevelComplete;
    public TextMeshProUGUI txtReward;
    public Button btnNoThanks;

    Level level;
    private void Start()
    {
        level = LevelManager.Instance.GetLevelInfo(AppDelegate.SharedManager().levelCounter);
        txtLevelComplete.text = "Level " + level.levelNo + " Complete";
        txtReward.text = "Reward " + level.rewardAmount;
        btnNoThanks.onClick.AddListener(() => NoThanksCallBack());
    }

    private void NoThanksCallBack()
    {
        UiManager.Instance.LoadMainPanel();
        AppDelegate.SharedManager().levelCounter += 1;

        Destroy(this.gameObject);
    }
}
