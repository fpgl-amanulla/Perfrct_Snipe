using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSlider : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject player;

    private CameraMotor cameraMotor;
    private void Start()
    {
        GetComponent<Slider>().onValueChanged.AddListener(delegate { CheckeValueChanger(); });

        cameraMotor = mainCamera.GetComponent<CameraMotor>();
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
    }

    public void Selected()
    {
        cameraMotor.isEnable = false;
        //Debug.Log("Selected");
    }

    public void DeSelected()
    {
        cameraMotor.isEnable = true;
        Vector3 rot = mainCamera.transform.eulerAngles;
        cameraMotor.rotX = rot.x;
        cameraMotor.rotY = player.transform.rotation.y;
        //Debug.Log("DeSelected");
    }
}
