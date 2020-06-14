using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scope : MonoBehaviour
{
    private Animator animator;
    public GameObject weaponCamera;

    public GameObject imgScope;

    private float scopedTime = .15f;
    private bool isScopeOut = true;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            animator.SetBool("Scoped", true);
            if (isScopeOut)
            {
                isScopeOut = false;
                StartCoroutine(ScopeIn());
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            isScopeOut = true;
            animator.SetBool("Scoped", false);
            ScopeOut();
        }
    }

    public IEnumerator ScopeIn()
    {
        yield return new WaitForSeconds(scopedTime);
        imgScope.SetActive(true);
        weaponCamera.SetActive(false);
    }

    public void ScopeOut()
    {
        imgScope.SetActive(false);
        weaponCamera.SetActive(true);
    }
}
