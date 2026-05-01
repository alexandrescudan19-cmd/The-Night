using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class block : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) // Dacă apeși SPACE
        {
            animator.SetTrigger("block");  // Activează animația de atac
            Debug.Log("Am apăsat atac!"); // Test: vezi în Console dacă funcționează
        }
    }

}
