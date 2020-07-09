using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(ToTutorialTwo());
    }

    IEnumerator ToTutorialTwo()
    {
        yield return new WaitForSeconds(7); // Timer is for 9 seconds because the story timeline rolls for 9 seconds
        SceneManager.LoadScene(7);
    }
}
