using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class NewMonoBehaviourScript : MonoBehaviour
{

    public void pressed()
    {
        SceneManager.LoadScene("Apartment scene");
    }
}