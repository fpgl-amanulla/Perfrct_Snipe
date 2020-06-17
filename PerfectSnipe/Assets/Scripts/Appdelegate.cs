using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppDelegate
{
    public static AppDelegate Instance = null;

    public int levelCounter = 0;

    public static AppDelegate SharedManager()
    {
        if (Instance == null)
            Instance = AppDelegate.Create();
        return Instance;
    }

    private static AppDelegate Create()
    {
        AppDelegate ret = new AppDelegate();
        if (ret != null && ret.Init())
        {
            return ret;
        }
        else
        {
            return null;
        }
    }

    private bool Init()
    {
        return true;
    }
}
