using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryBoard : MonoBehaviour
{
    private const int DURATION = 7;
    void Start()
    {

    }

    void Update()
    {
        StartCoroutine(ToNextScene());
    }

    private IEnumerator ToNextScene()
    {
        yield return new WaitForSeconds(DURATION);
        SceneManager.LoadScene(2);
    }
}
