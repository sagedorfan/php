using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("Skull Reference")]
    public GameObject skullObject;

    [Header("Buttons to Tint")]
    public Image[] menuButtons; 

    [Header("Colors")]
    public Color normalColor = Color.white; 
    public Color hoverColor = Color.gray;  

    void Start()
    {
       
        if (skullObject != null)
        {
            skullObject.SetActive(false);
        }
    }

   
    public void OnPlayHoverEnter()
    {
        if (skullObject != null) skullObject.SetActive(true);

       
        foreach (Image btnImage in menuButtons)
        {
            if (btnImage != null) btnImage.color = hoverColor;
        }
    }

    public void OnPlayHoverExit()
    {
        if (skullObject != null) skullObject.SetActive(false);

        
        foreach (Image btnImage in menuButtons)
        {
            if (btnImage != null) btnImage.color = normalColor;
        }
    }

    public void PlayGame()
    {
   
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
