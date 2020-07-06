using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PanelGame : MonoBehaviour
{
    public static PanelGame Instance;

    public TextMeshProUGUI txtObjective;

    public TextMeshProUGUI txtCompletedObjectives;
    public TextMeshProUGUI txtNumOfBullet;

    Level level;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void IniatializePanel()
    {
        AppDelegate appDelegate = AppDelegate.SharedManager();
        level = LevelManager.Instance.GetLevelInfo(appDelegate.levelCounter);
        txtObjective.text = "Find " + level.totalVictim + "  " + appDelegate.selectedDinoName + " shark and tranquilize them";
        UpdateGamePanel();
        UpDateNumOfBullet();
    }

    public void UpdateGamePanel()
    {
        txtCompletedObjectives.text = "Objective Completed: " + Score.SharedManager().GetCurrentScore().ToString();
    }
    public void UpDateNumOfBullet()
    {
        txtNumOfBullet.text = AppDelegate.SharedManager().numOfBullet.ToString();
    }
}

