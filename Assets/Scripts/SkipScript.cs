using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipScript : MonoBehaviour
{
    public void Skip()
    {
        SceneManager.LoadScene((int) GameSceneId.GAME_SCENE);
    }

    public void SkipStory()
    {
        SceneManager.LoadScene((int) GameSceneId.MENU);
    }
}
