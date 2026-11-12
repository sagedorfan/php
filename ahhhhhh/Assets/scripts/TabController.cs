using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabController : MonoBehaviour
{
    public Image[] tabImages;
    public GameObject[] pages;
    void Start()
    {
        ActivateTab(0);
    }

   
public void ActivateTab (int tabNo)
    {
        for (int i = 0; i < tabImages.Length; i++)
        {
            if (i == tabNo)
            {
                tabImages[i].color = Color.white;
                pages[i].SetActive(true);
            }
            else
            {
                tabImages[i].color = Color.gray;
                pages[i].SetActive(false);
            }
        }
    }
}