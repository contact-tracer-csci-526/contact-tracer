using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static int level;

    void Awake () {
        DontDestroyOnLoad(this);
    }

    public void PlayGame(int l)
    {
        level = l;
        MoveToScene(level);
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    // public void HandleClickMenu()
    // {
    //     SceneManager.LoadScene((int) GameSceneId.TUTORIAL_OR_GAME_NAVIGATOR);
    // }

    private void MoveToScene(int level)
    {
        GameLevel gameLevel = (GameLevel)level;

        switch (gameLevel) {
            case GameLevel.TUTORIAL_1:
                SceneManager.LoadScene((int)GameSceneId.TUTORIAL1_STORY_LINE);
                break;

            case GameLevel.TUTORIAL_2:
                SceneManager.LoadScene((int)GameSceneId.TUTORIAL2_STORY_LINE);
                break;

            case GameLevel.TUTORIAL_3:
                SceneManager.LoadScene((int)GameSceneId.TUTORIAL3_STORY_LINE);
                break;

            case GameLevel.NORMAL_1:
                SceneManager.LoadScene((int)GameSceneId.SCORE_BREAKDOWN);
                break;

             case GameLevel.NORMAL_2:
                SceneManager.LoadScene((int)GameSceneId.GAME_SCENE);
                break;

            
            

              case GameLevel.YEAR_2020:
                SceneManager.LoadScene((int)GameSceneId.GAME_SCENE);
                break;

            default:
                SceneManager.LoadScene((int)GameSceneId.GAME_SCENE);
                break;
        }
    }
}
