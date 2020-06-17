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
    public GameObject bulletImpactFX;

    [Header("Material")]
    public Material victimDied;

    public float impactForce = 20;
    private float scopedInFOV = 15f;
    private float defaultFOV = 60f;
    private float scopedTime = .15f;
    private bool isScopeOut = true;
    private bool isAimTxtActive = true;

    public float refreshTime = 0.2f;
    float refreshDelta;

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

        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, 100f))
        {
            Instantiate(bulletImpactFX, hit.transform.position, Quaternion.LookRotation(hit.normal));
            if (hit.collider.CompareTag("Victim"))
            {
                //GameObject newBullet = Instantiate(bullet, mainCamera.transform.position, Quaternion.identity);
                //newBullet.GetComponent<Rigidbody>().AddForce(Vector3.forward * 1500);
                if (hit.rigidbody == null)
                {
                    hit.collider.tag = "Untagged";
                    hit.transform.gameObject.AddComponent(typeof(Rigidbody));
                    hit.transform.GetComponent<Victim>().sequence.Pause();
                    hit.rigidbody.AddRelativeForce(-hit.normal * impactForce);
                    hit.transform.GetComponent<Renderer>().material = victimDied;

                    AddScore(1);
                }
            }
        }
    }

    public void AddScore(int _score)
    {
        score += _score;
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
        yield return new WaitForSeconds(2.5f);
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
