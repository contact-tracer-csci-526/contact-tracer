using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial1 : MonoBehaviour
{
    private const int DURATION = 4;

    void Start()
    {

    }

    void Update()
    {
        StartCoroutine(ToTutorialOne());
    }

    IEnumerator ToTutorialOne()
    {
        yield return new WaitForSeconds(DURATION);
        SceneManager.LoadScene((int) GameSceneId.GAME_SCENE);
    }
}
