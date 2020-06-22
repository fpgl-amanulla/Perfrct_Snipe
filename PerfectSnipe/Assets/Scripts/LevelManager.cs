using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level
{
    public int levelNo;
    public int totalVictim;
    public int rewardAmount;
    public VictimType victimType;
    public GameObject levelPrefab;
}

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public int levelToLoad = 0;
    public GameObject environment;
    public List<Level> levelList = new List<Level>();

    private GameObject level;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        AppDelegate.SharedManager().levelCounter = levelToLoad;
        LoadLevel(levelToLoad);
    }

    public void LoadLevel(int levelNo) => level = Instantiate(levelList[levelNo].levelPrefab, environment.transform);
    public void DestroyLevel() => Destroy(level);

    public Level GetLevelInfo(int levelNo) => levelList[levelNo];
}
