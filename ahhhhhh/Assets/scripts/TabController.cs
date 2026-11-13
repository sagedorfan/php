using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class TabController : MonoBehaviour
{
    [SerializeField] private Image[] tabImages;
    [SerializeField] private GameObject[] pages;

    private int currentTab = 0;

    private void Start()
    {
        ActivateTab(0);
    }

    private void Update()
    {
        if (Keyboard.current == null) return;

        
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            NextTab();
        }
        
        else if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            PreviousTab();
        }
    }

    public void NextTab()
    {
        int nextIndex = (currentTab + 1) % pages.Length;
        ActivateTab(nextIndex);
    }

    public void PreviousTab()
    {
        int prevIndex = (currentTab - 1 + pages.Length) % pages.Length;
        ActivateTab(prevIndex);
    }

    public void ActivateTab(int tabNo)
    {
        if (tabNo < 0 || tabNo >= pages.Length) return;

        currentTab = tabNo;

        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(false);
            if (i < tabImages.Length)
            {
                tabImages[i].color = Color.gray;
            }
        }

        pages[tabNo].SetActive(true);
        if (tabNo < tabImages.Length)
        {
            tabImages[tabNo].color = Color.white;
        }
    }
}
