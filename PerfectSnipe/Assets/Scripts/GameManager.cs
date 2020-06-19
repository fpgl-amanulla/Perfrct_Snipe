using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    public GameObject weaponHolder;
    public bool isLevelComplete = false;

    private AppDelegate appDelegate;

    void Awake()
    {
        if (Instance == null) Instance = this;
        appDelegate = AppDelegate.SharedManager();
    }
}
