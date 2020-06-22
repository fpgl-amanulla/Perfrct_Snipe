using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score
{
    public int score { get; private set; }

    public static Score Instance = null;

    public static Score SharedManager()
    {
        if (Instance == null)
        {
            Instance = Score.Create();
        }
        return Instance;
    }

    public int GetCurrentScore() => score;

    public void SetScore(int value) => score = value;
    public void AddScore(int value)
    {
        score += value;
        GameManager.Instance.CheckLevelComplete();
    }
    public void ResetScore() => score = 0;

    private static Score Create()
    {
        Score ret = new Score();
        if (ret != null && ret.Init())
        {
            return ret;
        }
        return null;
    }

    private bool Init()
    {
        return true;
    }

    public void TakeDamage(int value)
    {
        score -= value;
        GameManager.Instance.CheckLevelComplete();
    }
}
