using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial3 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(ToTutorialThree());
    }

    IEnumerator ToTutorialThree()
    {
        yield return new WaitForSeconds(7); // Timer is for 9 seconds because the story timeline rolls for 9 seconds
        SceneManager.LoadScene(7);
    }
}