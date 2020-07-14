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
             GameManager.CurrentGameState = GameState.Restart;
                MainMenu.level = 102;
                Time.timeScale = 1f;
                SceneManager.LoadScene((int) GameSceneId.GAME_SCENE);
                break;
            case GameLevel.NORMAL_2:
            GameManager.CurrentGameState = GameState.Restart;
                MainMenu.level = 202;
                Time.timeScale = 1f;
                SceneManager.LoadScene((int) GameSceneId.GAME_SCENE);
                break;
            case GameLevel.YEAR_2020:

            default:
                // GameManager.CurrentGameState = GameState.Restart;
                // MainMenu.level = 102;
                // Time.timeScale = 1f;
                // SceneManager.LoadScene((int) GameSceneId.GAME_SCENE);
                break;
        }
    }

    public void NextTutorial()
    {
        Time.timeScale = 1f;

        if (MainMenu.level == (int) GameLevel.TUTORIAL_1)
        {
            GameManager.CurrentGameState = GameState.Restart;
            MainMenu.level = 2;
            Time.timeScale = 1f;
            SceneManager.LoadScene((int) GameSceneId.TUTORIAL2_STORY_LINE);
        }
        else if (MainMenu.level == (int) GameLevel.TUTORIAL_2)
        {
            GameManager.CurrentGameState = GameState.Restart;
            MainMenu.level = 3;
            Time.timeScale = 1f;
            SceneManager.LoadScene((int) GameSceneId.TUTORIAL3_STORY_LINE);
        }
    }

    public void LoadMenu()
    {
        GameManager.CurrentGameState = GameState.Restart;
        Time.timeScale = 1f;
        SceneManager.LoadScene((int) GameSceneId.MENU);
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
