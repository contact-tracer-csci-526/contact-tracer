using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreMechanism : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Test());
    }

    void Update()
    {
        int score = PlayerPrefs.GetInt("score");
    }

    IEnumerator Test()
     {
        yield return new WaitForSeconds(2);
        int score = PlayerPrefs.GetInt("score");
        SceneManager.LoadScene(1);
    }
}
