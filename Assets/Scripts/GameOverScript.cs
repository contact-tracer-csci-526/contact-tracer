using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    public static int level;

    public void NextLevel()
    {
        Time.timeScale = 1f;

        if (MainMenu.level == 1)
        {
            GameManager.CurrentGameState = GameManager.GameState.Restart;
            MainMenu.level = 2;
            Time.timeScale = 1f;
            SceneManager.LoadScene("gameScene");
        }
        else if (MainMenu.level == 2)
        {
        GameManager.CurrentGameState = GameManager.GameState.Restart;
        MainMenu.level = 3;
        Time.timeScale = 1f;
        SceneManager.LoadScene("gameScene");
        }
    }

    public void LoadMenu()
    {
        GameManager.CurrentGameState = GameManager.GameState.Restart;
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
        GameManager.CurrentGameState = GameManager.GameState.Restart;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
