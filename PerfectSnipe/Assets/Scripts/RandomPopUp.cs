using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class RandomPopUp : MonoBehaviour
{
    public List<string> textList = new List<string>();
    public TextMeshProUGUI txtPopUp;
    private void OnEnable()
    {
        txtPopUp.text = textList[Random.Range(0, textList.Count)];
    }
}
