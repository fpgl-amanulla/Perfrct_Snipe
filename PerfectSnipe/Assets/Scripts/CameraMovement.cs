using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float REFRESH_TIME = 0.1f;
    float refreshDelta = 0.0f;
    private Vector3 touchStartPos;
    [SerializeField]
    [Range(5, 20)]
    float smoothingTime = 10.0f; //decrease value to move faster
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStartPos = Input.mousePosition;
            refreshDelta = 0.0f;
        }
        if (Input.GetMouseButton(0))
        {

            refreshDelta += Time.deltaTime;
            Vector3 diff = (Input.mousePosition - touchStartPos);
            Vector3 finalPos = transform.position + diff;

            if (refreshDelta >= REFRESH_TIME)
            {

                refreshDelta = 0.0f;
                touchStartPos = Input.mousePosition;
            }

            transform.position = Vector3.Lerp(transform.position, new Vector3(finalPos.x, finalPos.y, transform.position.z), Time.deltaTime / smoothingTime);

        }
        if (Input.GetMouseButtonUp(0))
        {
            transform.localPosition = new Vector3(0, 0, 0);
        }
    }
}
