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

     private int frameCount = 0;

     private int timeRemaining = 10; //sec


    Text statusText;
    Text timerText;

    // Use this for initialization
    void Start()
    {
        CDS = GameObject.Find ("DelayedStart").GetComponent<DelayedStartScript> ();
        Cells = GameObject.FindGameObjectsWithTag("Cell");
        Virus = GameObject.Find("Virus");
        statusText = GameObject.Find("Status").GetComponent<Text>();
        statusText.enabled = false;
        timerText = GameObject.Find("Timer").GetComponent<Text>();
        timerText.text = "" + timeRemaining;
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
                    StartCoroutine(operateTimer());
                }
                break;
            case GameState.Playing:
                timerText.text = "" + timeRemaining;
                //calc infection rate
                GameObject[] Uninfected = GameObject.FindGameObjectsWithTag("Cell");
                GameObject[] Infected = GameObject.FindGameObjectsWithTag("Virus");
                int IR = (100 * Infected.Length) / (Uninfected.Length + Infected.Length);
                if (IR >= infectionLimit) { //end condition
                    CurrentGameState = GameState.Over;
                    StopCoroutine(operateTimer());
                    Time.timeScale = 0;
                    statusText.enabled = true;
                    //save time/score
                } else if (timeRemaining == 0) { //win condition
                    CurrentGameState = GameState.Over;
                    StopCoroutine(operateTimer());
                    Time.timeScale = 0;
                    statusText.text = "Congrats!\n You survived!";
                    statusText.enabled = true;
                }
                break;

            case GameState.Over:
                //display "Game Over: Infection rate exceeded" (flash on every 10th frame?)
                //display score/time-survived
                frameCount++;

                //return to start scene (wait on button input for direction to switch out of Game over state)
                if (frameCount > 300) {
                  //StartCoroutine(ToSplash());
                  //print("transition started");
                  CurrentGameState =  GameState.Start;
                  Time.timeScale = 1;
                  SceneManager.LoadScene(1);
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

    IEnumerator operateTimer()
    {
      while (true) {
        yield return new WaitForSeconds(1);
        timeRemaining--;
      }
    }

}
