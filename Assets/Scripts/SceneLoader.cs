using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }   
    }

    void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene("Victory");
    }
}
