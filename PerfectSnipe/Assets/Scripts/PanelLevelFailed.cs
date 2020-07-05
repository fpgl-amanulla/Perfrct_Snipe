using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelLevelFailed : MonoBehaviour
{
    public Button btnRestart;

    void Start()
    {
        btnRestart.onClick.AddListener(() => RestartCallBack());
    }

    private void RestartCallBack()
    {
        LevelManager.Instance.ResetLevel();
        LevelManager.Instance.LoadLevel();
        Score.SharedManager().ResetScore();
        PanelGame.Instance.IniatializePanel();
        PanelBulletManager.Instance.InitializeBullletBar();
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
