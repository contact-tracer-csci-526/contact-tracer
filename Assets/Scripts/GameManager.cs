using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameState CurrentGameState = GameState.Start;

    public static GameObject[] Cells;

    public static GameObject Virus;

     private DelayedStartScript CDS;

     public int infectionLimit = 50; //percent


    Text statusText;

    // Use this for initialization
    void Start()
    {
        CDS = GameObject.Find ("DelayedStart").GetComponent<DelayedStartScript> ();
        Cells = GameObject.FindGameObjectsWithTag("Cell");
        Virus = GameObject.Find("Virus");
        statusText = GameObject.Find("Status").GetComponent<Text>();
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
                        Cells[i].GetComponent<BallScript>().StartBall();
                    }
                    Virus.GetComponent<BallScript>().StartBall();
                    CurrentGameState = GameState.Playing;
                }
                break;
            case GameState.Playing:
                //calc infection rate
                GameObject[] Uninfected = GameObject.FindGameObjectsWithTag("Cell");
                GameObject[] Infected = GameObject.FindGameObjectsWithTag("Virus");
                int IR = (100 * Infected.Length) / (Uninfected.Length + Infected.Length);
                if (IR > infectionLimit) {
                    CurrentGameState = GameState.Over;
                    print("GAME OVER");
                    Time.timeScale = 0;
                    //save time/score
                }
                break;

            case GameState.Over:
                //display "Game Over: Infection rate exceeded" (flash on every 10th frame?)
                //display score/time-survived
                void onGUI() {
                  GUI.Label(new Rect(0,0,Screen.width,Screen.height),"Game Over, Loser");
                }

                //return to start scene (wait on button input for direction to switch out of Game over state)
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
}
