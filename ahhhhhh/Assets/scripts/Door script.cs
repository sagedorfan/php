using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Doorscript : MonoBehaviour
{
    Animator animator;

    [SerializeField] private Animator doorAnimator;

    private bool openDoor = false;

    //// Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{
        
    //}

    // Update is called once per frame
    void Update()
    {
        

       //if (Input.DoorOpen.IsPressed())
       // {
       //     doorAnimator.SetBool("isOpen", true);
       //     doorAnimator.SetBool("isClosed", false);
       // }
       // else
       // {
       //     doorAnimator.SetBool("isOpen", false);
       //     doorAnimator.SetBool("isClosed", true);
       // }



    }
}
