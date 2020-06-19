using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private GameObject newBullet;

    [Header("Values")]
    [SerializeField] private float impactForce = 20;
    [SerializeField] private float explosionForce = 300f;
    [SerializeField] private float scopedInFOV = 25f;
    [SerializeField] private float defaultFOV = 60f;
    [Tooltip("Time to scoped in")]
    [SerializeField] private float scopedTime = 0.15f;
    private float refreshTime = 0.01f;
    private float refreshDelta;

    private bool isAimTxtActive = true;
    private bool isScopeOut = true;
    private bool shoot = false;

    private RaycastHit hit;
    private ShootType shootType;
    private BulletType bulletType;

    Sequence sequence;
    public int score { get; set; }
    #endregion
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {

        if (GameManager.Instance.isLevelComplete)
            return;

        InputHandler();
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
                StartCoroutine(ScopeIn());
            }
        }
        if (refreshDelta > refreshTime)
        {
            if (Input.GetMouseButtonUp(0))
            {
                shoot = true;
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
                shootType = ShootType.Victim;
                hit.transform.GetComponent<Victim>().sequence.Pause();
                SpawnBullet();
                int val = UnityEngine.Random.Range(0, 100);
                bulletType = val % 2 == 0 ? BulletType.Normal : BulletType.WithCamera;

                if (bulletType == BulletType.WithCamera)
                {
                    weaponCamera.SetActive(false);
                    imgScope.SetActive(false);
                    Vector3 camPos = new Vector3(0, 0, -1.5f);
                    sequence = DOTween.Sequence();
                    sequence.AppendInterval(.1f).Append(mainCamera.transform.DOMove(hit.point + camPos, .4f));
                    Time.timeScale = .6f;
                }
            }
            else if (hit.collider.CompareTag("Destroyable"))
            {
                shootType = ShootType.Destroyable;
                SpawnBullet();
            }
            else if (hit.collider.CompareTag("Explosive"))
            {
                shootType = ShootType.Explosive;

                SpawnBullet();
                SoundManager.Instance.PlayExplosiveSound();

            }
            else
            {
                shoot = false;
                ScopeOut();
            }
        }
        else
        {
            shoot = false;
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

                if (hit.rigidbody == null)
                {
                    hit.transform.gameObject.AddComponent(typeof(Rigidbody));
                }
                hit.collider.tag = "Untagged";
                hit.transform.GetComponent<Renderer>().material.color = Color.gray;
                hit.rigidbody.AddRelativeForce(-hit.normal * impactForce);
                AddScore(1);
                break;
            case ShootType.Destroyable:
                Destroy(hit.transform.gameObject);
                break;
            case ShootType.Explosive:

                Instantiate(FxManager.Instance.explosiveImpactFX, hit.transform.position, Quaternion.LookRotation(hit.normal));

                Collider[] colliders = Physics.OverlapSphere(hit.point, 10);

                foreach (var item in colliders)
                {
                    if (!item.gameObject.CompareTag("Static"))
                    {
                        if (item.GetComponent<Rigidbody>() == null)
                        {
                            item.gameObject.AddComponent(typeof(Rigidbody));
                            Rigidbody rb = item.GetComponent<Rigidbody>();
                            rb.AddExplosionForce(explosionForce, hit.point, 50);
                            if (item.gameObject.CompareTag("Victim"))
                            {
                                AddScore(1);
                                item.transform.GetComponent<Victim>().sequence.Pause();
                                item.transform.GetComponent<Renderer>().material.color = Color.grey;
                                StartCoroutine(WaitToDestroy(item.gameObject, .5f));
                            }
                        }
                    }
                }
                Destroy(hit.transform.gameObject);
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
                shoot = false;
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
        sequence.Kill();
        Time.timeScale = 1.0f;
        mainCamera.transform.DOLocalMove(new Vector3(0, 0, 0), 0f);
        shoot = false;
        ScopeOut();
    }

    public IEnumerator WaitToDestroy(GameObject go, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(go);
    }

    public void AddScore(int _score)
    {
        score += _score;
        //Debug.Log("Score " + score);
        int currentLevel = AppDelegate.SharedManager().levelCounter;
        if (score >= LevelManager.Instance.GetLevelInfo(currentLevel).totalVictim)
        {
            score = 0;
            GameManager.Instance.isLevelComplete = true;
            StartCoroutine(WaitToLoadLevelComplete());
        }

    }

    IEnumerator WaitToLoadLevelComplete()
    {
        yield return new WaitForSeconds(2.0f);
        UiManager.Instance.LoadLevelComplete();
        LevelManager.Instance.DestroyLevel();
        GameManager.Instance.weaponHolder.SetActive(false);
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
        animator.SetBool("Scoped", false);
        mainCamera.fieldOfView = defaultFOV;
        imgScope.SetActive(false);
        weaponCamera.SetActive(true);
    }
}
