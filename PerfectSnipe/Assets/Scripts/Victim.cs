using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;

public class Victim : MonoBehaviour
{
    public Animator victimAnim;

    public GameObject playerCanvas;
    public Slider imgFill;
    public VictimType victimType;
    public int bossHealth;

    public Vector3 startPosition;
    public Vector3 endPosition;
    public float durationTime;
    public bool startWalk;
    public bool startDance;


    public Transform visual;

    public Sequence sequence { get; private set; }

    private Vector3 currentDirection = Vector3.forward;
    private Vector3 deviation;
    private Vector3 moveDirection = Vector3.forward;
    private WaitForSeconds pointDelay;
    public float speed = .1f;
    [SerializeField] private float maxDragDistance = 40f;
    [SerializeField] private float MaxRadius;
    [SerializeField] private float MinRadius;
    public float movementSmoothing;
    [SerializeField] private float rotationSmoothing;

    public MeshCollider meshCollider;
    private Vector3 initaiPos;

    public bool isDied { get; set; }

    public int health = 1;
    private void Start()
    {
        initaiPos = this.transform.position;
        StartCoroutine(GeneratePointRoutine());

        if (startWalk)
            StartMove();
        else
        {
            if (startDance)
                victimAnim.SetBool("StartDance", true);
        }

        if (victimType == VictimType.Boss)
        {
            Score.SharedManager().SetScore(bossHealth);
            imgFill.maxValue = bossHealth;
            imgFill.value = bossHealth;
        }
        imgFill.maxValue = health;
        imgFill.value = health;
    }
    #region PerfectSnipe
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
            if (!isDied)
            {
                isDied = true;
                this.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.color = Color.gray;
                if (this.GetComponent<Rigidbody>() == null)
                    this.gameObject.AddComponent<Rigidbody>();
                this.GetComponent<Rigidbody>().AddForce(collision.transform.forward * 10);
                victimAnim.enabled = false;
                Score.SharedManager().AddScore(1);
                //Destroy(collision.gameObject);
            }
        }
    }

    public void UpdateHealthBar()
    {
        health -= 1;
        imgFill.value = health;
    }
    #endregion

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

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.DrawSphere(transform.position, MaxRadius);
    //}

    public void VictimDie()
    {
        if (health <= 0)
        {
            this.gameObject.tag = "Untagged";
            Score.SharedManager().AddScore(1);
            victimAnim.SetBool("Died", true);
            meshCollider.enabled = false;
            playerCanvas.SetActive(false);
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
        Vector3 randomPoint = new Vector3(initaiPos.x + UnityEngine.Random.Range(MinRadius, MaxRadius),
            initaiPos.y + UnityEngine.Random.Range(MinRadius, MaxRadius),
            initaiPos.z + UnityEngine.Random.Range(MinRadius, MaxRadius));

        return randomPoint;
    }

}
