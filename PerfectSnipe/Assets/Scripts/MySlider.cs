using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MySlider : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject player;

    private void Start()
    {
        GetComponent<Slider>().onValueChanged.AddListener(delegate { CheckeValueChanger(); });
    }

    public void OnEnable()
    {
        Vector3 rot = player.transform.eulerAngles;
        if (rot.y > 90)
        {
            rot.y -= 360;
        }
        GetComponent<Slider>().value = rot.y;
    }

    private void CheckeValueChanger()
    {
        Vector3 rot = player.transform.eulerAngles;
        rot.y = GetComponent<Slider>().value;
        player.transform.eulerAngles = rot;
        //mainCamera.transform.Rotate(Vector3.up * GetComponent<Slider>().value);
    }

    public void Selected()
    {
        CameraMotor.Instance.isEnable = false;
        //Debug.Log("Selected");
    }

    public void DeSelected()
    {
        CameraMotor.Instance.isEnable = true;
        Vector3 rot = mainCamera.transform.eulerAngles;
        CameraMotor.Instance.rotX = rot.x;
        CameraMotor.Instance.rotY = player.transform.rotation.y;
        //Debug.Log("DeSelected");
    }
}
