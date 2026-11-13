using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour
{

    public void pressed()
    {
        SceneManager.LoadScene("Office Scene");
    }
}