using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMotor : MonoBehaviour
{
    public GameObject player;

    private Vector3 lastPos;
    private Vector3 deltapos;
    public float Speed;

    public float rotX = 0;
    public float rotY = 0;

    public bool isEnable = true;

    private void Update()
    {

        if (!GameManager.Instance.isGameStarted || isEnable == false || AppDelegate.SharedManager().numOfBullet <= 0)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            lastPos = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            deltapos = Input.mousePosition - lastPos;

            rotX -= deltapos.y * Time.deltaTime * Speed * 1;
            rotY = deltapos.x * Time.deltaTime * Speed * 1;

            rotX = Mathf.Clamp(rotX, -15, 15);

            transform.localRotation = Quaternion.Euler(rotX, 0, 0);

            player.transform.Rotate(Vector3.up * rotY);

            lastPos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {

        }
    }
}
