
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultNavigation : MonoBehaviour
{
    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void RunAgain()
    {
        SceneManager.LoadScene("KalkulatorCpuShadule");
    }
}
