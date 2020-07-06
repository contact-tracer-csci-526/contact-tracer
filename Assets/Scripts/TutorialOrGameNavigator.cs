using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* To be changed */
public class TutorialOrGameNavigator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
