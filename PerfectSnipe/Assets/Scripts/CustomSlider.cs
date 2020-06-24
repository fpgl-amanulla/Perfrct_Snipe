using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEngine;

public class CustomSlider : MonoBehaviour
{
    public Transform knob;
    public int[] valueRange;
    public int decimalPoint;
    public int initialSliderPercent;

    private Vector3 targetPos;
    private float sliderParcent;
    private float sliderLength;
    public LayerMask touchInputMask;
    private void Start()
    {
        sliderLength = GetComponent<BoxCollider>().size.x - .4f;
        sliderParcent = initialSliderPercent;

        targetPos = knob.position + Vector3.right * (sliderLength / -2 + sliderLength * sliderParcent);
        knob.position = targetPos;
    }
    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, touchInputMask))
        {
            if (Input.GetMouseButton(0))

                targetPos = new Vector3(hit.point.x, targetPos.y, targetPos.z);
        }

        knob.position = Vector3.Lerp(knob.position, targetPos, Time.deltaTime * 7);

        sliderParcent = Mathf.Clamp01(knob.position.x + sliderLength / 2) / sliderLength;

    }

}
