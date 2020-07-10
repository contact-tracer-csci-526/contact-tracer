using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial3 : MonoBehaviour
{
    private const int DURATION = 3;

    void Start()
    {

    }

    void Update()
    {
        StartCoroutine(ToTutorialThree());
    }

    IEnumerator ToTutorialThree()
    {
        yield return new WaitForSeconds(DURATION);
        SceneManager.LoadScene((int) GameSceneId.GAME_SCENE);
    }
}