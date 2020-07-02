using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;

public class Victim : MonoBehaviour
{
    public Animator victimAnim;

    public GameObject canvas;
    public Slider imgFill;
    public VictimType victimType;
    public Transform visual;

    public Sequence sequence { get; private set; }

    private Vector3 currentDirection = Vector3.forward;
    private Vector3 deviation;
    private Vector3 moveDirection = Vector3.forward;
    private WaitForSeconds pointDelay;
    [HideInInspector]
    public float speed = .01f;
    private float maxDragDistance = 40f;
    private float MaxRadius = 5;
    private float MinRadius = -5;
    private float movementSmoothing = 150;
    private float rotationSmoothing = 150;

    public MeshCollider meshCollider;
    private Vector3 initialPos;

    public bool isDied { get; set; }

    [Header("Dino Data")]
    public String dinoName;
    public int dinoId;
    public int health = 1;
    public bool isSelected { get; set; }
    private void Start()
    {
        initialPos = this.transform.position;
        StartCoroutine(GeneratePointRoutine());
        imgFill.maxValue = health;
        imgFill.value = health;
    }

    public void UpdateHealthBar()
    {
        health -= 5;
        imgFill.value = health;
    }

    private void Update()
    {
        HandleVictimMovement();
    }

    private void HandleVictimMovement()
    {
        if (isDied)
            return;

        currentDirection = Vector3.zero;
        currentDirection = new Vector3(Mathf.Lerp(currentDirection.x, moveDirection.x, movementSmoothing), 0f, Mathf.Lerp(currentDirection.y, moveDirection.y, movementSmoothing));
        deviation = currentDirection * speed * maxDragDistance * Time.deltaTime;
        transform.position += deviation;
        visual.rotation = Quaternion.Slerp(visual.rotation, Quaternion.LookRotation(currentDirection) * Quaternion.Euler(new Vector3(0, 360, 0)), rotationSmoothing);
        //transform.SetPositionAndRotation(transform.position + deviation, transform.rotation);
        //playerRigidbody.MovePosition(playerRigidbody.position + (deviation * 2f));
    }


    public void VictimDie()
    {
        if (health <= 0)
        {
            this.gameObject.tag = "Untagged";
            victimAnim.SetBool("Died", true);
            victimAnim.SetTrigger("isDied");
            meshCollider.enabled = false;
            transform.DORotateQuaternion(Quaternion.Euler(0, 0, 180), 1f);
            if (AppDelegate.SharedManager().selectedDinoId == dinoId.ToString() && isSelected)
                Score.SharedManager().AddScore(1);
            canvas.SetActive(false);
            Destroy(this.gameObject, 2.0f);
        }
        else
        {
            isDied = false;
        }

    }
    IEnumerator GeneratePointRoutine()
    {
        while (true)
        {
            moveDirection = (GetRandomPoint() - transform.position).normalized;
            pointDelay = new WaitForSeconds(UnityEngine.Random.Range(4, 7));
            yield return pointDelay;
        }
    }

    public Vector3 GetRandomPoint()
    {
        //Vector3 randomPoint = (Vector3)UnityEngine.Random.insideUnitCircle * UnityEngine.Random.Range(MinRadius, MaxRadius);
        Vector3 randomPoint = new Vector3(initialPos.x + UnityEngine.Random.Range(MinRadius, MaxRadius),
            initialPos.y + UnityEngine.Random.Range(MinRadius, MaxRadius),
            initialPos.z + UnityEngine.Random.Range(MinRadius, MaxRadius));

        return randomPoint;
    }
}
