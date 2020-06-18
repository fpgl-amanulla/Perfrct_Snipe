using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WeaponController : MonoBehaviour
{
    private Animator animator;

    public Camera mainCamera;
    public GameObject weaponCamera;
    public GameObject imgScope;
    public GameObject bullet;
    [Header("FX")]
    public GameObject bulletImpactFX;
    public GameObject explosiveImpactFX;

    [Header("Material")]
    public Material victimDied;

    public float impactForce = 20;
    private float scopedInFOV = 25f;
    private float defaultFOV = 60f;
    private float scopedTime = .15f;
    private bool isScopeOut = true;
    private bool isAimTxtActive = true;

    public float refreshTime = 0.2f;
    float refreshDelta;
    private float explosionForce = 300;

    public int score { get; set; }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {

        if (GameManager.Instance.isLevelComplete)
            return;

        if (Input.GetMouseButton(0))
        {
            refreshDelta += Time.deltaTime;

            if (isAimTxtActive || UiManager.Instance.tapToAim.activeSelf)
            {
                UiManager.Instance.tapToAim.SetActive(false);
                isAimTxtActive = false;
            }

            animator.SetBool("Scoped", true);
            if (isScopeOut)
            {
                isScopeOut = false;
                StartCoroutine(ScopeIn());
            }
        }
        else
        {
            animator.SetBool("Scoped", false);
            ScopeOut();
        }
        if (Input.GetMouseButtonUp(0))
        {

            if (refreshDelta > refreshTime)
            {
                refreshDelta = 0;
                Shoot();
            }

            isScopeOut = true;
            animator.SetBool("Scoped", false);
            ScopeOut();
        }
    }

    private void Shoot()
    {
        RaycastHit hit;

        SoundManager.Instance.PlayShootSound();
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, 100f))
        {
            Instantiate(bulletImpactFX, hit.point, Quaternion.LookRotation(hit.normal));
            if (hit.collider.CompareTag("Victim"))
            {
                UiManager.Instance.ShowPopUptext();
                if (hit.rigidbody == null)
                {
                    hit.collider.tag = "Untagged";
                    hit.transform.gameObject.AddComponent(typeof(Rigidbody));
                    hit.transform.GetComponent<Victim>().sequence.Pause();
                    hit.transform.GetComponent<Renderer>().material = victimDied;
                    hit.rigidbody.AddRelativeForce(-hit.normal * impactForce);

                    AddScore(1);
                }
            }

            if (hit.collider.CompareTag("Explosive"))
            {
                SoundManager.Instance.PlayExplosiveSound();
                Instantiate(explosiveImpactFX, hit.transform.position, Quaternion.LookRotation(hit.normal));

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
                                item.transform.GetComponent<Renderer>().material = victimDied;
                                StartCoroutine(WaitToDestroy(item.gameObject, .5f));
                            }
                        }
                    }
                }
                Destroy(hit.transform.gameObject);
            }
        }
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
        mainCamera.fieldOfView = defaultFOV;
        imgScope.SetActive(false);
        weaponCamera.SetActive(true);
    }
}
