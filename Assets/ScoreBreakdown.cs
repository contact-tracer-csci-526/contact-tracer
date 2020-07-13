using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreBreakdown : MonoBehaviour
{
    private const int DURATION = 4;

    void Start()
    {

    }

    void Update()
    {
        StartCoroutine(ToLevelOne());
    }

    IEnumerator ToLevelOne()
    {
        yield return new WaitForSeconds(DURATION);
        SceneManager.LoadScene(7);
    }
}
