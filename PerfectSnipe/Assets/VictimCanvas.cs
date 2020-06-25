using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VictimCanvas : MonoBehaviour
{
    public TextMeshProUGUI txtName;
    public Image imgIndicator;

    public void Initialize(bool isShowIndicator)
    {
        txtName.text = GetComponentInParent<Victim>().dinoName;
        if (isShowIndicator)
            imgIndicator.gameObject.SetActive(true);
        else
            imgIndicator.gameObject.SetActive(false);

    }
}
