using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    // Start is called before the first frame update
    public int level;
    public string Level1,Level2,scene;
    
    //  void Awake () {
         
    //     // stops object from automtically getting destroyed on loading another scene
    //     DontDestroyOnLoad (this);
    // }
  
        public void NextLevel()
    {   
         //Debug.Log(level);
        Time.timeScale = 1f;
       scene= SceneManager.GetActiveScene().name;
       Debug.Log(scene);
        if(scene == "Level1")
        {
        
        Debug.Log(scene);
        GameManager.getInstance().currentGameState= GameState.Restart;
        Time.timeScale = 1f; 
        SceneManager.LoadScene("Level2");
        
        }
        if (scene == "Level2")
        {
          
            GameManager.getInstance().currentGameState = GameState.Restart;
            Time.timeScale = 1f;
           SceneManager.LoadScene("Level3");
            
            
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