using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreBreakdown : MonoBehaviour
{
    private const int DURATION = 3;

    void Start()
    {

    }

    void Update()
    {
        StartCoroutine(ToLevelOne());
    }

    IEnumerator ToLevelOne()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(7);
    }
}
