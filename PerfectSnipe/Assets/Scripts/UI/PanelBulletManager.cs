using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PanelBulletManager : MonoBehaviour
{
    public static PanelBulletManager Instance = null;

    public List<Image> imgBullets = new List<Image>();

    private void Start()
    {
        if (Instance == null) Instance = this;

        imgBullets = transform.GetComponentsInChildren<Image>().ToList();
        imgBullets.Remove(imgBullets[0]);

        InitializeBullletBar();
    }

    public void InitializeBullletBar()
    {
        int numOfbullet = AppDelegate.SharedManager().numOfBullet;
        for (int i = 0; i < imgBullets.Count; i++)
        {
            if (i < imgBullets.Count - numOfbullet)
                imgBullets[i].color = Color.black;
            else
                imgBullets[i].color = Color.white;
        }
    }
    public void ResetBulletBar()
    {
        for (int i = 0; i < imgBullets.Count; i++)
        {
            imgBullets[i].color = Color.black;
        }
        InitializeBullletBar();
    }
}
