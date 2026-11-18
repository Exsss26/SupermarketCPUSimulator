using UnityEngine;
using UnityEngine.InputSystem;

public class PressAnyKeyToMenu : MonoBehaviour
{
    public MainMenuController menuController;

    void Update()
    {
        if (menuController == null) return;

        if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
        {
            menuController.ShowMainMenu();
        }

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            menuController.ShowMainMenu();
        }
    }
}