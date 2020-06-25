using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PanelGame : MonoBehaviour
{
    public static PanelGame Instance;

    public TextMeshProUGUI txtObjective;

    public TextMeshProUGUI txtCompletedObjectives;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void IniatializePanel()
    {
        AppDelegate appDelegate = AppDelegate.SharedManager();
        Level level = LevelManager.Instance.GetLevelInfo(appDelegate.levelCounter);
        txtObjective.text = "Find " + level.totalVictim + "  " + appDelegate.selectedDinoName + " Dino and tranquilize them";
        UpdateGamePanel();
    }

    public void UpdateGamePanel()
    {
        txtCompletedObjectives.text = "Objective Completed: " + Score.SharedManager().GetCurrentScore().ToString();
    }
}

