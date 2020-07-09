using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* To be changed */
public class TutorialOrGameNavigator : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        StartCoroutine(ToNextScene());
    }

    IEnumerator ToNextScene()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(3);
    }
}
