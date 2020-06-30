using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static int  level;
    void Awake () {
         
        // stops object from automtically getting destroyed on loading another scene
        DontDestroyOnLoad (this);
    }
    public void PlayGame(int l)
    {
        level = l;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    public void QuitGame()
    {
    Debug.Log("QUIT");
    Application.Quit();
    }
}