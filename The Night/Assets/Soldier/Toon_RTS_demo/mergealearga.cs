using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mergealearga : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) // Dacă apeși SPACE
        {
            animator.SetTrigger("merge");  // Activează animația de atac
        }
    }

}