
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject environmentGroup;
    public GameObject mainMenuPanel;

    void Start()
    {
        environmentGroup.SetActive(true);
        mainMenuPanel.SetActive(false);
    }

    public void ShowMainMenu()
    {
        environmentGroup.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}
