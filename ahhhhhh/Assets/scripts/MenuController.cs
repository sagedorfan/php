using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuController : MonoBehaviour
{
    public GameObject menuCanvas;
   
    void Start()
    {
        menuCanvas.SetActive(false);
    }


    void Update()
    {
        if (Keyboard.current.fKey.isPressed)
        {
            menuCanvas.SetActive(!menuCanvas.activeSelf);
        }
    }
}
