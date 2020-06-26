using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    // Start is called before the first frame update
   // public static bool GamePause = false;

    public GameObject pauseMenuUI, PauseButton;
    


    // Update is called once per frame
   

    public void Resume()
    {
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
        PauseButton.SetActive(true);
       
        //GamePause = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        PauseButton.SetActive(false);
        Time.timeScale = 0f;
        //GamePause = true;
    }

    public void LoadMenu()
    {
        //Debug.Log("Menu");
        GameManager.CurrentGameState = GameManager.GameState.Restart;
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
         GameManager.CurrentGameState = GameManager.GameState.Restart;
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
