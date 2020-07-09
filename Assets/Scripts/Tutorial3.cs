using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial3 : MonoBehaviour
{
    private const int DURATION = 7;

    void Start()
    {

    }

    void Update()
    {
        StartCoroutine(ToTutorialThree());
    }

    IEnumerator ToTutorialThree()
    {
        yield return new WaitForSeconds(7);
        SceneManager.LoadScene(7);
    }
}