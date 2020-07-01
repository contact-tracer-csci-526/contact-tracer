using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    // Start is called before the first frame update
    public static int level;

    public void NextLevel()
    {
        int level = GameManager.getInstance().gameConfig.level;
        Time.timeScale = 1f;
        if (level == 1) {
            GameManager.getInstance().currentGameState = GameState.Restart;
            GameManager.getInstance().gameConfig.level = 2;
            Time.timeScale = 1f;
            SceneManager.LoadScene("gameScene");
        }
        else if(level==2) {

        //Debug.Log(scene);
        GameManager.getInstance().currentGameState= GameState.Restart;
        GameManager.getInstance().gameConfig.level = 2;
        Time.timeScale = 1f;
        SceneManager.LoadScene("gameScene");

        }
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
