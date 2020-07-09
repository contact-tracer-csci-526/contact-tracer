using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* To be changed */
public class TutorialOrGameNavigator : MonoBehaviour
{
    

     public void LoadGame()
    {
        SceneManager.LoadScene("menu");
    }

}