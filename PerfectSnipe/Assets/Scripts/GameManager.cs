using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    AppDelegate appDelegate;
    public GameObject weaponHolder;
    public bool isLevelComplete = false;
    void Start()
    {
        if (Instance == null) Instance = this;
        appDelegate = AppDelegate.SharedManager();
    }
}
