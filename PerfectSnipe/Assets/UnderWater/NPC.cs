using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NPC : MonoBehaviour
{
    public enum VictimType
    {
        Surfer,
        Swimmer_Male,
        Swimmer_Female,
        Idle,
        FatLady,
        Police,
        Fish
    }

    [Header("Properties")]
    public VictimType type;
    public float currentSpeed;
    public float normalSpeed;
    public float panicSpeed;
    public float rotateSpeed = 0.25f;
    private int randomDirection;
    private Vector3 direction;
    private bool flipped;
    [HideInInspector] public bool isPanicked;
    private WaitForSeconds panicDelay;

    [Header("Attached Components")]
    public GameObject PanicUI;
    public GameObject PanicTrail;
    public Animator anim;
    private SkinnedMeshRenderer[] SMR;

    // Anim IDs
    private readonly int PANIC_ANIM_ID = Animator.StringToHash("isPaniced");


    void Start()
    {
        SMR = GetComponentsInChildren<SkinnedMeshRenderer>();
        //UpdateSMR(false);
        panicDelay = new WaitForSeconds(4.0f);
        randomDirection = Random.Range(-1, 2);

        currentSpeed = normalSpeed;
        panicSpeed = normalSpeed * 2.5f /*1.5f*/;

        if (type == VictimType.FatLady)
        {
            anim = gameObject.GetComponentInChildren<Animator>();
        }
    }

    private void Update()
    {
        //if (GameManager.Instance.isGameOver || !GameManager.Instance.isGameStarted)
        //{
        //    return;
        //}

        if (GameManager.Instance.isLevelComplete)
        {
            return;
        }

        switch (type)
        {
            case VictimType.Idle:
                transform.Rotate(new Vector3(0, 360 * randomDirection, 0) * Time.deltaTime * rotateSpeed);
                break;
            case VictimType.Police:
                MoveRight();
                break;
            case VictimType.FatLady:
                if (isPanicked)
                {
                    MoveBackward();
                }

                else
                {
                    MoveForword();
                }

                break;
            default:
                MoveForword();
                break;
        }
    }

    public void MoveForword()
    {
        direction = Vector3.forward * currentSpeed * Time.deltaTime;
        transform.Translate(direction);
    }

    public void MoveBackward()
    {
        direction = Vector3.back * currentSpeed * Time.deltaTime;
        transform.Translate(direction);
    }

    public void MoveRight()
    {
        direction = Vector3.right * currentSpeed * Time.deltaTime;
        transform.Translate(direction);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (type != VictimType.Idle)
        {
            if (other.CompareTag("Wall"))
            {
                Flip();
            }

        }
    }

    private void Flip()
    {
        if (!flipped)
        {
            if (type == VictimType.FatLady)
            {
                if (anim)
                {
                    anim.SetBool(PANIC_ANIM_ID, true);
                }
            }
            else
            {
                transform.Rotate(Vector3.up * -180f);
                if (type == VictimType.Surfer)
                {
                    if (PanicTrail)
                    {
                        PanicTrail.SetActive(true);
                        currentSpeed = panicSpeed;
                    }
                }
            }

            StartCoroutine(PanicTime());
            flipped = true;
        }
    }

    IEnumerator PanicTime()
    {
        yield return panicDelay;

        if (PanicUI)
        {
            PanicUI.SetActive(false);
            isPanicked = false;
            if (type == VictimType.FatLady)
            {
                if (anim)
                {
                    anim.SetBool(PANIC_ANIM_ID, false);
                }
            }
        }

        currentSpeed = normalSpeed;
        if (type == VictimType.Surfer)
        {
            if (PanicTrail)
            {
                PanicTrail.SetActive(false);
            }

        }

        flipped = false;
    }

    public void UpdateSMR(bool state)
    {
        int count = SMR.Length;
        for (int i = 0; i < count; i++)
        {
            SMR[i].enabled = state;
        }
    }
}
