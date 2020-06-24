using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MySlider : MonoBehaviour
{
    public GameObject mainCamera;

    public void OnEnable()
    {
        Debug.Log(mainCamera.transform.localRotation.y / Time.deltaTime);
        GetComponent<Slider>().value = mainCamera.transform.localRotation.y / Time.deltaTime;
    }

    public void SliderChanged(float value)
    {
        Quaternion q = mainCamera.transform.rotation;
        Quaternion z = q;
        z.x = q.x;
        z.y = value * Time.deltaTime;
        z.z = q.z;
        mainCamera.transform.rotation = Quaternion.Lerp(q, z, .05f);
    }
}
