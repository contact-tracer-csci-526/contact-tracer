using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject PauseButton;

    public void Resume()
    {
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
        PauseButton.SetActive(true);
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        PauseButton.SetActive(false);
        Time.timeScale = 0f;
    }

    public void LoadMenu()
    {
        GameManager.getInstance().currentGameState = GameState.Restart;
        Time.timeScale = 1f;
        SceneManager.LoadScene("menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Restart()
    {
        Time.timeScale = 1f;
         GameManager.getInstance().currentGameState = GameState.Restart;
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
