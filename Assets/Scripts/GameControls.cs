using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControls : MonoBehaviour
{
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Quit()
    {
        Debug.Log("Quit game");
        Application.Quit();
    }
}
