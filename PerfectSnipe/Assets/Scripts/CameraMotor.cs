using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMotor : MonoBehaviour
{
    private Touch initTouch = new Touch();

    private float rotX = 0f;
    private float rotY = 0f;
    private Vector3 origRot;

    public float rotSpeed = 0.1f;
    public float dir = -1;

    public float REFRESH_TIME = 0.1f;
    float refreshDelta = 0.0f;

    private Vector3 inititalPos;

    void Start()
    {
        origRot = transform.eulerAngles;
        rotX = origRot.x;
        rotY = origRot.y;
    }


    void FixedUpdate()
    {
        //if (Application.isEditor)
        //{
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        refreshDelta = 0;
        //        inititalPos = Input.mousePosition;
        //    }
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        refreshDelta += Time.deltaTime;

        //        float deltaX = Input.mousePosition.x - inititalPos.x;
        //        float deltaY = Input.mousePosition.y - inititalPos.y;
        //        rotX -= deltaY * Time.deltaTime * rotSpeed * dir;
        //        rotY += deltaX * Time.deltaTime * rotSpeed * dir;
        //        rotX = Mathf.Clamp(rotX, -720f, 720f);
        //        transform.eulerAngles = new Vector3(rotX, rotY, 0f) * Time.deltaTime;


        //        if (refreshDelta >= REFRESH_TIME)
        //        {
        //            refreshDelta = 0.0f;
        //            inititalPos = Input.mousePosition;
        //        }
        //    }
        //    if (Input.GetMouseButtonUp(0))
        //    {
        //        transform.localPosition = new Vector3(0, 0, 0);
        //    }
        //}
        foreach (Touch touch in Input.touches)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                initTouch = new Touch();
            }
            else
            {
                if (touch.phase == TouchPhase.Began)
                {
                    initTouch = touch;
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    refreshDelta += Time.deltaTime;

                    float deltaX = initTouch.position.x - touch.position.x;
                    float deltaY = initTouch.position.y - touch.position.y;
                    rotX -= deltaY * Time.deltaTime * rotSpeed * dir;
                    rotY += deltaX * Time.deltaTime * rotSpeed * dir;
                    rotX = Mathf.Clamp(rotX, -720f, 720f);
                    transform.eulerAngles = new Vector3(rotX, rotY, 0f) * Time.deltaTime;


                    if (refreshDelta >= REFRESH_TIME)
                    {
                        refreshDelta = 0.0f;
                        initTouch = touch;
                    }
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    initTouch = new Touch();
                }
            }
        }
    }
}
