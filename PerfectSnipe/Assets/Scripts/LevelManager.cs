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
}

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public int levelToLoad = 0;
    public GameObject environment;
    public List<Level> levelList = new List<Level>();

    public GameObject dinoPrefab;
    public GameObject dinos;
    public List<GameObject> allDino = new List<GameObject>();
    private void Awake()
    {
        if (Instance == null) Instance = this;
        AppDelegate.SharedManager().levelCounter = levelToLoad;
        LoadLevel();
    }

    public void LoadLevel()
    {
        allDino = new List<GameObject>();
        Level level = GetLevelInfo(AppDelegate.SharedManager().levelCounter);
        for (int i = 0; i < level.totalVictim + 1; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-15, 15), 0, Random.Range(-7, 7));
            GameObject g = Instantiate(dinoPrefab, dinos.transform);
            g.transform.localPosition = pos;
            allDino.Add(g);
        }
    }

    public void ResetLevel()
    {
        for (int i = 0; i < allDino.Count; i++)
        {
            Destroy(allDino[i]);
        }
    }
    public Level GetLevelInfo(int levelNo) => levelList[levelNo];
}
