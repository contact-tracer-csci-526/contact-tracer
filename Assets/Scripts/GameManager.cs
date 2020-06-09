using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameState CurrentGameState = GameState.Start;

    public static GameObject[] Cells;

    public static GameObject Virus;

     private DelayedStartScript CDS;

     private int infectionLimit = 100; //percent

     //private bool transitionStarted = false;

     private int frameCount = 0;


    Text statusText;

    // Use this for initialization
    void Start()
    {
        CDS = GameObject.Find ("DelayedStart").GetComponent<DelayedStartScript> ();
        Cells = GameObject.FindGameObjectsWithTag("Cell");
        Virus = GameObject.Find("Virus");
        statusText = GameObject.Find("Status").GetComponent<Text>();
        statusText.enabled = false;
    }


//    private bool InputTaken()
//    {
//        return Input.touchCount > 0 || Input.GetMouseButtonUp(0);
//    }


    // Update is called once per frame
    void Update()
    {
        switch (CurrentGameState)
        {
            case GameState.Start:
                if (CDS.counterDownDone == true)
                {
                    for (int i = 0; i < Cells.Length; i++) {
                        Cells[i].GetComponent<Ball>().StartBall();
                    }
                    Virus.GetComponent<Ball>().StartBall();
                    CurrentGameState = GameState.Playing;
                }
                break;
            case GameState.Playing:
                //calc infection rate
                GameObject[] Uninfected = GameObject.FindGameObjectsWithTag("Cell");
                GameObject[] Infected = GameObject.FindGameObjectsWithTag("Virus");
                int IR = (100 * Infected.Length) / (Uninfected.Length + Infected.Length);
                if (IR >= infectionLimit) {
                    CurrentGameState = GameState.Over;

                    Time.timeScale = 0;
                    statusText.enabled = true;
                    //save time/score
                }
                break;

            case GameState.Over:
                //display "Game Over: Infection rate exceeded" (flash on every 10th frame?)
                //display score/time-survived
                print("GAME OVER");
                frameCount++;

                //return to start scene (wait on button input for direction to switch out of Game over state)
                if (frameCount > 1000) {
                  //StartCoroutine(ToSplash());
                  print("transition started");
                  SceneManager.LoadScene(0);
                  //transitionStarted = true;
                  //Time.timeScale = 1;
                }

                break;

            default:
                break;
        }
    }

    public enum GameState
    {
        Start,
        Playing,
        Over
    }

    // IEnumerator ToSplash()
    //  {
    //      //yield return new WaitForSeconds(5);
    //      SceneManager.LoadScene(0);
    //  }

}
