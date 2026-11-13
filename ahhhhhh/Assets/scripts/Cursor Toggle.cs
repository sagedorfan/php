using UnityEngine;
using UnityEngine.InputSystem;

public class CursorToggle : MonoBehaviour
{
    void Update()
    {
        // Toggle cursor visibility and lock state with the 'H' key (or change KeyCode to what you prefer)
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            SetCursorVisible(!Cursor.visible);
        }
    }

    public void SetCursorVisible(bool visible)
    {
        Cursor.visible = visible;

        if (visible)
        {
            Cursor.lockState = CursorLockMode.None; // Free the cursor
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked; // Lock/hide the cursor to the center
        }
    }
}