using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipScript : MonoBehaviour
{
    // Start is called before the first frame update

    public void Skip() 
    {

    
    SceneManager.LoadScene((int) GameSceneId.GAME_SCENE);

    }
    
}
