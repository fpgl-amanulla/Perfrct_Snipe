using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Victim : MonoBehaviour
{
    public Vector3 startPosition;
    public Vector3 endPosition;
    public float durationTime;
    public bool isAnimate;

    public Sequence sequence { get; private set; }

    private void Start()
    {
        if (isAnimate)
            StartMove();
    }

    private void StartMove()
    {
        sequence = DOTween.Sequence();
        sequence.Append(transform.DOLocalMove(endPosition, durationTime))
            .Append(transform.DOLocalMove(startPosition, durationTime)).SetLoops(-1);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Slayer"))
        {
            this.GetComponent<Renderer>().material.color = Color.gray;
            Destroy(collision.gameObject);
        }
    }
}
