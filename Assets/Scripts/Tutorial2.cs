using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial2 : MonoBehaviour
{
    private const int DURATION = 7;

    void Start()
    {

    }

    void Update()
    {
        StartCoroutine(ToTutorialTwo());
    }

    IEnumerator ToTutorialTwo()
    {
        yield return new WaitForSeconds(DURATION);
        SceneManager.LoadScene(7);
    }
}
