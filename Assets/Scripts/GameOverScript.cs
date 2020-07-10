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
        GameLevel gameLevel = (GameLevel) MainMenu.level;

        switch (gameLevel) {
            case GameLevel.NORMAL_1:
            case GameLevel.NORMAL_2:
            default:
                GameManager.CurrentGameState = GameState.Restart;
                MainMenu.level = 102;
                Time.timeScale = 1f;
                SceneManager.LoadScene((int) GameSceneId.GAME_SCENE);
                break;
        }
    }

    public void LoadMenu()
    {
        GameManager.CurrentGameState = GameState.Restart;
        Time.timeScale = 1f;
        SceneManager.LoadScene(GameSceneId.MENU);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        GameManager.CurrentGameState = GameState.Restart;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
