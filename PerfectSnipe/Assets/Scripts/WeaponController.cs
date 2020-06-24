using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponController : MonoBehaviour
{
    #region Variables
    private Animator animator;
    [Header("Gameobjects reference")]
    public Camera mainCamera;
    public GameObject weaponCamera;
    public GameObject imgScope;
    public GameObject bulletPrefab;
    public GameObject firePoint;

    public GameObject Slider;

    private GameObject newBullet;

    [Header("Values")]
    [SerializeField] private float impactForce = 20;
    [SerializeField] private float explosionForce = 300f;
    [SerializeField] private float scopedInFOV = 20f;
    [SerializeField] private float defaultFOV = 60f;
    [Tooltip("Time to scoped in")]
    [SerializeField] private float scopedTime = 0.15f;
    private float refreshTime = 0.01f;
    private float refreshDelta;

    private bool isAimTxtActive = true;
    private bool isScopeOut = true;
    //private bool shoot = false;

    private RaycastHit hit;
    private ShootType shootType;
    private BulletType bulletType;

    Sequence sequence;
    public int score { get; set; }

    private bool islooking = false;
    #endregion
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {

        if (GameManager.Instance.isLevelComplete)
            return;

        if (EventSystem.current.IsPointerOverGameObject() && EventSystem.current.currentSelectedGameObject)
        {
            return;
        }
        foreach (Touch touch in Input.touches)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                return;
            }
        }
        InputHandler();
    }

    private void LateUpdate()
    {
        if (islooking)
            mainCamera.transform.LookAt(hit.point + Vector3.forward);
    }

    private void InputHandler()
    {
        if (Input.GetMouseButtonDown(0)) return;

        if (Input.GetMouseButton(0))
        {
            refreshDelta += Time.deltaTime;

            if (isAimTxtActive || UiManager.Instance.tapToAim.activeSelf)
            {
                UiManager.Instance.tapToAim.SetActive(false);
                isAimTxtActive = false;
            }

            if (isScopeOut)
            {
                isScopeOut = false;
                animator.SetBool("Scoped", true);
                Slider.SetActive(false);
                StartCoroutine(ScopeIn());
            }
        }
        if (refreshDelta > refreshTime)
        {
            if (Input.GetMouseButtonUp(0))
            {
                //shoot = true;
                Shoot();
            }
        }
        else
        {
            //StopAllCoroutines();
            //if (shoot == false)
            //{
            //    shoot = false;
            //    ScopeOut();
            //}
        }
    }

    private void Shoot()
    {
        refreshDelta = 0;
        bulletType = BulletType.Normal;
        SoundManager.Instance.PlayShootSound();
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, 100f))
        {
            if (hit.collider.CompareTag("Victim"))
            {
                hit.transform.GetComponentInParent<Victim>().isDied = true;
                hit.transform.GetComponentInParent<Victim>().UpdateHealthBar();

                SpawnBullet();
                int val = UnityEngine.Random.Range(0, 100);
                bulletType = val % 2 == 0 ? BulletType.Normal : BulletType.WithCamera;

                if (bulletType == BulletType.WithCamera)
                {
                    weaponCamera.SetActive(false);
                    imgScope.SetActive(false);
                    Vector3 camPos = new Vector3(0, 0, -10.5f);
                    sequence = DOTween.Sequence();
                    islooking = true;
                    sequence.AppendInterval(.1f).Append(mainCamera.transform.DOMove(hit.point + camPos, .4f));
                    Time.timeScale = .4f;
                }
            }
            else
            {
                Instantiate(FxManager.Instance.bulletImpactFXDefault, hit.point, Quaternion.LookRotation(hit.normal));
                //shoot = false;
                ScopeOut();
            }
        }
        else
        {
            //shoot = false;
            ScopeOut();
        }
    }

    private void SpawnBullet()
    {
        newBullet = Instantiate(bulletPrefab, firePoint.transform.position, Quaternion.identity, this.transform);
        sequence = DOTween.Sequence();
        sequence.Append(newBullet.transform.DOMove(hit.point, .3f)).OnComplete(CompleteDone);
    }

    private void CompleteDone()
    {
        Instantiate(FxManager.Instance.bulletImpactFX, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(newBullet);
        UiManager.Instance.ShowPopUptext();
        switch (shootType)
        {
            case ShootType.Victim:
                hit.transform.GetComponentInParent<Victim>().VictimDie();
                //hit.transform.gameObject.tag = "Untagged";
                //hit.transform.GetComponent<MeshCollider>().enabled = false;
                //hit.transform.GetComponentInParent<Victim>().victimAnim.SetBool("Died", true);
                //hit.transform.GetComponentInParent<Victim>().enabled = false;
                //Score.SharedManager().AddScore(1);
                break;
            case ShootType.Other:

                break;

            default:

                break;
        }

        switch (bulletType)
        {
            case BulletType.Normal:
                sequence.Kill();
                //shoot = false;
                ScopeOut();
                break;
            case BulletType.WithCamera:
                StartCoroutine(ResetMainCamPos());
                break;
            default:
                Time.timeScale = 1.0f;
                break;
        }
    }

    private IEnumerator ResetMainCamPos()
    {
        yield return new WaitForSeconds(.5f);
        islooking = false;
        sequence.Kill();
        Time.timeScale = 1.0f;
        mainCamera.transform.DOLocalMove(new Vector3(0, 0, 0), 0f);
        //shoot = false;
        ScopeOut();
    }

    public IEnumerator WaitToDestroy(GameObject go, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(go);
    }

    public IEnumerator ScopeIn()
    {
        yield return new WaitForSeconds(scopedTime);
        mainCamera.fieldOfView = scopedInFOV;
        imgScope.SetActive(true);
        weaponCamera.SetActive(false);
    }

    public void ScopeOut()
    {
        isScopeOut = true;
        Slider.SetActive(true);
        animator.SetBool("Scoped", false);
        mainCamera.fieldOfView = defaultFOV;
        //Vector3 v = mainCamera.transform.eulerAngles;
        //v.z = 0;
        //mainCamera.transform.eulerAngles = v;
        imgScope.SetActive(false);
        weaponCamera.SetActive(true);
    }
}
