using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ataca : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Dacă apeși SPACE
        {
            animator.SetTrigger("Attack");  // Activează animația de atac
            Debug.Log("Am apăsat atac!"); // Test: vezi în Console dacă funcționează
        }
    }

}
