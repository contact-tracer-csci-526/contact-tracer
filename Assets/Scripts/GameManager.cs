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
                break;
            default:
                break;
        }
    }

    public enum GameState
    {
        Start,
        Playing,
        Won,
        LostALife,
        LostAllLives
    }
}
