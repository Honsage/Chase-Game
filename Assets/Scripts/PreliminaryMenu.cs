using UnityEngine;
using UnityEngine.SceneManagement;

public class PreliminaryMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void ExitGame()
    {
        Debug.Log("Exit out of game!");
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
