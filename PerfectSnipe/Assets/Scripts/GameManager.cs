using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    public GameObject weaponHolder;
    public GameObject mainCamera;
    public bool isLevelComplete = false;

    private AppDelegate appDelegate;

    void Awake()
    {
        if (Instance == null) Instance = this;
        appDelegate = AppDelegate.SharedManager();
    }

    public void CheckLevelComplete()
    {
        //Debug.Log("Score " + score);
        int currentLevel = AppDelegate.SharedManager().levelCounter;
        int score = Score.SharedManager().GetCurrentScore();
        Level levelInfo = LevelManager.Instance.GetLevelInfo(currentLevel);
        if (levelInfo.victimType == VictimType.Boss)
        {
            if (score <= 0)
            {
                isLevelComplete = true;
                StartCoroutine(WaitToLoadLevelComplete());
            }
        }
        else
        {
            if (score >= levelInfo.totalVictim)
            {
                isLevelComplete = true;
                StartCoroutine(WaitToLoadLevelComplete());
            }
        }

    }

    IEnumerator WaitToLoadLevelComplete()
    {
        yield return new WaitForSeconds(2.0f);
        UiManager.Instance.LoadLevelComplete();
        LevelManager.Instance.ResetLevel();
        weaponHolder.SetActive(false);
    }
}
