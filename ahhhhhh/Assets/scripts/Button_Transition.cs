using UnityEngine;
using UnityEngine;
using UnityEngine.SceneManagement; //This is the scene manager, which will enable the button to transition to the next scene


public class Button_Transition : MonoBehaviour
{
    public void NextScene() //This is the function that will be called when the button is pressed, it will load the next scene in the build settings
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //This will load the next scene in the build settings

    }
}