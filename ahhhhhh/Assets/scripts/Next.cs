using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Next : MonoBehaviour
{

    public void pressed()
    {
        SceneManager.LoadScene("Main Menu");
    }
}