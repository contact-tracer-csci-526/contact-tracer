using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public int  level;
    void Awake () {
        // stops object from automtically getting destroyed on loading another scene
        DontDestroyOnLoad (this);
    }
    public void PlayGame(int l)
    {
        level = l;
        if(l==1)
        {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else if (l==2)
        {
            SceneManager.LoadScene("Level2");
        }
        else 
        {
            SceneManager.LoadScene("Level3");
        }
    }
    
    public void QuitGame()
    {
    Debug.Log("QUIT");
    Application.Quit();
    }
}
