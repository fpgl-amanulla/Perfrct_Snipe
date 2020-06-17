using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level
{
    public int levelNo;
    public int totalVictim;
    public int rewardAmount;
    public GameObject levelPrefab;
}

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public GameObject environment;
    public List<Level> levelList = new List<Level>();

    private GameObject level;

    private void Start()
    {
        if (Instance == null) Instance = this;

        LoadLevel(0);
    }

    public void LoadLevel(int levelNo) => level = Instantiate(levelList[levelNo].levelPrefab, environment.transform);
    public void DestroyLevel() => Destroy(level);

    public Level GetLevelInfo(int levelNo) => levelList[levelNo];
}
