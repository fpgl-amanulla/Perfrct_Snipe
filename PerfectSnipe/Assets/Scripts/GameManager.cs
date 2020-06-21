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
        if (score >= LevelManager.Instance.GetLevelInfo(currentLevel).totalVictim)
        {
            isLevelComplete = true;
            StartCoroutine(WaitToLoadLevelComplete());
        }

    }

    IEnumerator WaitToLoadLevelComplete()
    {
        yield return new WaitForSeconds(2.0f);
        UiManager.Instance.LoadLevelComplete();
        LevelManager.Instance.DestroyLevel();
        weaponHolder.SetActive(false);
    }
}
