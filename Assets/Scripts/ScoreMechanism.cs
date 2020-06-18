using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreMechanism : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //DontDestroyOnLoad(gameObject);
        StartCoroutine(Test());
    }

    // Update is called once per frame
    void Update()
    {
        int score = PlayerPrefs.GetInt("score");
        Debug.Log(score);
       
    }

    IEnumerator Test()
     {
        yield return new WaitForSeconds(2);
        int score = PlayerPrefs.GetInt("score");
        Debug.Log(score);
        SceneManager.LoadScene(1);
        // DontDestroyOnLoad(this.gameObject);
    }
}
