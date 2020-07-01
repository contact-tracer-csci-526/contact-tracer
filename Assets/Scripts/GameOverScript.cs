using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    // Start is called before the first frame update
    public static int level;


    //  void Awake () {

    //     // stops object from automtically getting destroyed on loading another scene
    //     DontDestroyOnLoad (this);
    // }

        public void NextLevel()
    {
         //Debug.Log(level);
        Time.timeScale = 1f;
        Debug.Log(MainMenu.level);

    //    scene= SceneManager.GetActiveScene().name;
    //    Debug.Log(scene);
        if(MainMenu.level==1)
        {

        //Debug.Log(scene);
        GameManager.getInstance().currentGameState= GameState.Restart;
        MainMenu.level=2;
        Time.timeScale = 1f;
        SceneManager.LoadScene("gameScene");

        }
       else if(MainMenu.level==2)
        {

        //Debug.Log(scene);
        GameManager.getInstance().currentGameState= GameState.Restart;
        MainMenu.level=3;
        Time.timeScale = 1f;
        SceneManager.LoadScene("gameScene");

        }
    }
 public void LoadMenu()
    {
        //Debug.Log("Menu");
        GameManager.getInstance().currentGameState = GameState.Restart;
        Time.timeScale = 1f;
        SceneManager.LoadScene("menu");


        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        //Debug.Log("Quit");
        Application.Quit();
    }

    public void Restart()

    {
        Time.timeScale = 1f;
        GameManager.getInstance().currentGameState = GameState.Restart;
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}