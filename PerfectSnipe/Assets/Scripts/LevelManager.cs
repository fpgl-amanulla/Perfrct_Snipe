using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level
{
    public int levelNo;
    public int totalVictim;
    public int rewardAmount;
    public int numberOfBullet;
    public int dinoId;
    public VictimType victimType;
}

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public int levelToLoad = 0;
    public GameObject environment;
    public List<Level> levelList = new List<Level>();

    public List<GameObject> allDinoPrefab;
    public GameObject dinos;
    public List<GameObject> allDino = new List<GameObject>();
    private void Awake()
    {
        if (Instance == null) Instance = this;
        AppDelegate.SharedManager().levelCounter = levelToLoad;
        LoadLevel();
    }

    private void Start()
    {
        PanelGame.Instance.IniatializePanel();
    }

    public void LoadLevel()
    {
        allDino = new List<GameObject>();
        Level level = GetLevelInfo(AppDelegate.SharedManager().levelCounter);
        GameObject selectedDino = null;
        for (int i = 0; i < allDinoPrefab.Count; i++)
        {
            if (allDinoPrefab[i].name == level.dinoId.ToString())
            {
                selectedDino = allDinoPrefab[i];
                break;
            }
        }

        string dinoId = selectedDino.GetComponent<Victim>().dinoId.ToString();
        string dinoName = selectedDino.GetComponent<Victim>().dinoName;
        AppDelegate.SharedManager().selectedDinoId = dinoId;
        AppDelegate.SharedManager().selectedDinoName = dinoName;

        for (int i = 0; i < level.totalVictim + 1; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-15, 15), 0, Random.Range(-7, 7));
            GameObject g = Instantiate(selectedDino, dinos.transform);
            g.GetComponent<Victim>().isSelected = true;
            g.GetComponent<Victim>().canvas.GetComponent<VictimCanvas>().Initialize(true);
            g.transform.localPosition = pos;
            allDino.Add(g);
        }

        for (int i = 0; i < Random.Range(1, 3); i++)
        {
            Vector3 pos = new Vector3(Random.Range(-15, 15), 0, Random.Range(-7, 7));
            GameObject g = Instantiate(allDinoPrefab[Random.Range(0, allDinoPrefab.Count)], dinos.transform);
            g.GetComponent<Victim>().canvas.GetComponent<VictimCanvas>().Initialize(false);
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
