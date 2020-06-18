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

    Level level;
    private void Start()
    {
        level = LevelManager.Instance.GetLevelInfo(AppDelegate.SharedManager().levelCounter);
        txtLevel.text = (AppDelegate.SharedManager().tempLevelCounter + 1).ToString();
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
        if (AppDelegate.SharedManager().levelCounter + 1 > 4)
        {
            AppDelegate.SharedManager().levelCounter = UnityEngine.Random.Range(0, 5);
        }
        else
            AppDelegate.SharedManager().levelCounter += 1;
        AppDelegate.SharedManager().tempLevelCounter += 1;

        Destroy(this.gameObject);
    }
}
