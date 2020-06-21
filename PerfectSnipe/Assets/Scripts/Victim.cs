using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Victim : MonoBehaviour
{
    public Animator victimAnim;

    public Vector3 startPosition;
    public Vector3 endPosition;
    public float durationTime;
    public bool startWalk;
    public bool startDance;

    public Sequence sequence { get; private set; }

    private void Start()
    {
        if (startWalk)
            StartMove();
        else
        {
            if (startDance)
                victimAnim.SetBool("StartDance", true);
        }
    }

    private void StartMove()
    {
        victimAnim.SetBool("StartWalk", true);
        sequence = DOTween.Sequence();
        sequence.Append(transform.DOLocalMove(endPosition, durationTime))
            .Append(transform.DOLocalRotateQuaternion(Quaternion.Euler(0, 180, 0), .2f))
            .Append(transform.DOLocalMove(startPosition, durationTime)).SetLoops(-1);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Slayer"))
        {
            this.GetComponent<Renderer>().material.color = Color.gray;
            Score.SharedManager().AddScore(1);
            GameManager.Instance.CheckLevelComplete();
            Destroy(collision.gameObject);
        }
    }
}
