using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance;

    public GameObject tapToAim;
    public GameObject levelCompletePrefab;
    public GameObject mainMenuPrefab;

    public TextMeshProUGUI txtLevelNo;

    void Start()
    {
        if (Instance == null) Instance = this;
        InitGamePanel();
    }

    void Update()
    {

    }

    public void LoadLevelComplete()
    {
        Instantiate(levelCompletePrefab, this.transform);
        txtLevelNo.gameObject.SetActive(false);
    }

    internal void LoadMainPanel()
    {
        Instantiate(mainMenuPrefab, this.transform);
    }
    public void InitGamePanel()
    {
        txtLevelNo.text = "Level " + (AppDelegate.SharedManager().levelCounter + 1).ToString();
    }
}
