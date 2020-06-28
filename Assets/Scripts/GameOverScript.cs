using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    // Start is called before the first frame update
    public int level;

        public void NextLevel()
    {   
        
        Time.timeScale = 1f;
        if(level==1)
        {
        Time.timeScale = 1f;
    
        GameManager.CurrentGameState = GameManager.GameState.Start;
        SceneManager.LoadScene("Level2");
        
        }
        if (level==2)
        {
          
            GameManager.CurrentGameState = GameManager.GameState.Start;
            SceneManager.LoadScene("Level3");
            
        }
        
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
       
        

