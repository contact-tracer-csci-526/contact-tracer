using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial1 : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        StartCoroutine(ToTuorialOne());
    }

    IEnumerator ToTuorialOne()
    {
        yield return new WaitForSeconds(6); // Timer is for 9 seconds because the story timeline rolls for 9 seconds
        SceneManager.LoadScene(5);
    }
}
