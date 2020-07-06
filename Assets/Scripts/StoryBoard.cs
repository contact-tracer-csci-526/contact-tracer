using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryBoard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }
    void Update()
    {
        StartCoroutine(ToNextScene());
    }

    IEnumerator ToNextScene()
    {
        yield return new WaitForSeconds(9); // Timer is for 9 seconds because the story timeline rolls for 9 seconds
        SceneManager.LoadScene(2);
    }
}
